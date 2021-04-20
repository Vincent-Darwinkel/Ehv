using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using User_Service.CustomExceptions;
using User_Service.Dal;
using User_Service.Enums;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.Models.HelperFiles;
using User_Service.Models.RabbitMq;
using User_Service.RabbitMq.Publishers;

namespace User_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly UserPublisher _producer;

        public UserLogic(IUserDal userDal, IMapper mapper, UserPublisher producer)
        {
            _userDal = userDal;
            _mapper = mapper;
            _producer = producer;
        }

        private bool UserModelValid(User user)
        {
            return !string.IsNullOrEmpty(user.Username) &&
                   user.AccountRole != AccountRole.Undefined &&
                   !string.IsNullOrEmpty(user.Email) &&
                   !string.IsNullOrEmpty(user.About) &&
                   user.Gender != Gender.Undefined;
        }

        /// <summary>
        /// Saves the user in the database
        /// </summary>
        /// <param name="user">The form data the user send</param>
        public async Task Register(User user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            /*UserDto dbUser = await _userDal.Find(user.Username, user.Email);
            if (dbUser != null)
            {
                throw new DuplicateNameException();
            }*/

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AccountRole = AccountRole.User;
            var userRabbitMq = _mapper.Map<UserRabbitMq>(user);
            userRabbitMq.Uuid = userDto.Uuid;

            _producer.Publish(userRabbitMq, RabbitMqRouting.AddUser);
            //await _userDal.Add(userDto);
        }

        /// <returns>All users in the database</returns>
        public async Task<List<UserDto>> All()
        {
            return await _userDal.All();
        }

        /// <summary>
        /// Finds all users which match the uuid in the collection
        /// </summary>
        /// <param name="uuidCollection">The uuid collection</param>
        /// <returns>The found users, null if nothing is found</returns>
        public async Task<List<UserDto>> Find(List<Guid> uuidCollection)
        {
            return await _userDal.Find(uuidCollection);
        }

        /// <summary>
        /// Finds the user by uuid
        /// </summary>
        /// <param name="uuid">The uuid to search for</param>
        /// <returns>The found user, null if nothing is found</returns>
        public async Task<UserDto> Find(Guid uuid)
        {
            return await _userDal.Find(uuid);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">The new user data</param>
        /// <param name="requestingUserUuid">the uuid of the user that made the request</param>
        public async Task Update(User user, Guid requestingUserUuid)
        {
            UserDto dbUser = await _userDal.Find(requestingUserUuid);
            await ValidateUpdateData(user, dbUser);

            dbUser.Username = user.Username;
            dbUser.Email = user.Email;
            dbUser.About = user.About;
            dbUser.Hobbies = _mapper.Map<List<UserHobbyDto>>(user.Hobbies);
            dbUser.FavoriteArtists = _mapper.Map<List<FavoriteArtistDto>>(user.FavoriteArtists);

            if (!string.IsNullOrEmpty(user.NewPassword) || dbUser.Email != user.Email)
            {
                var userRabbitMq = _mapper.Map<UserRabbitMq>(user);
                _producer.Publish(userRabbitMq, RabbitMqRouting.UpdateUser);
            }

            await _userDal.Update(dbUser);
        }

        /// <summary>
        /// Checks if the updated data is valid, if not an exception is thrown
        /// </summary>
        /// <param name="user">The new data</param>
        /// <param name="dbUser">The data from the database</param>
        private async Task ValidateUpdateData(User user, UserDto dbUser)
        {
            if (dbUser == null)
            {
                throw new UnprocessableException();
            }

            if (user.Email != dbUser.Email || user.Username != dbUser.Username)
            {
                bool userExists = await _userDal.Exists(user.Username, user.Email);
                if (userExists)
                {
                    throw new DuplicateNameException();
                }
            }
        }

        /// <summary>
        /// Deletes the user by uuid
        /// </summary>
        /// <param name="requestingUser">The user that made the request</param>
        /// <param name="userUuidToDeleteUuid">The uuid of the user to remove</param>
        public async Task Delete(UserDto requestingUser, Guid userUuidToDeleteUuid)
        {
            UserDto dbUserToDelete = await _userDal.Find(userUuidToDeleteUuid);
            if (dbUserToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            switch (requestingUser.AccountRole)
            {
                case AccountRole.SiteAdmin:
                    _producer.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.Admin when dbUserToDelete.AccountRole == AccountRole.User:
                    _producer.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.User when requestingUser.Uuid == userUuidToDeleteUuid:
                    _producer.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.Undefined:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new UnauthorizedAccessException();
        }
    }
}
