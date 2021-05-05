using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using User_Service.CustomExceptions;
using User_Service.Dal;
using User_Service.Enums;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.Models.HelperFiles;
using User_Service.Models.RabbitMq;
using User_Service.RabbitMq;
using User_Service.RabbitMq.Publishers;

namespace User_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;

        public UserLogic(IUserDal userDal, IMapper mapper, IPublisher publisher)
        {
            _userDal = userDal;
            _mapper = mapper;
            _publisher = publisher;
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

            UserDto dbUser = await _userDal.Find(user.Username, user.Email);
            if (dbUser != null)
            {
                throw new DuplicateNameException();
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AccountRole = AccountRole.User;
            userDto.Uuid = Guid.NewGuid();

            var userRabbitMq = _mapper.Map<UserRabbitMq>(user);
            userRabbitMq.Uuid = userDto.Uuid;

            _publisher.Publish(userRabbitMq, RabbitMqRouting.AddUser, RabbitMqExchange.UserExchange);
            await _userDal.Add(userDto);
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
            await CheckForDuplicatedUserData(user, dbUser);

            dbUser.Username = user.Username;
            dbUser.Email = user.Email;
            dbUser.About = user.About;
            dbUser.Hobbies = _mapper.Map<List<UserHobbyDto>>(user.Hobbies);
            dbUser.FavoriteArtists = _mapper.Map<List<FavoriteArtistDto>>(user.FavoriteArtists);

            if (!string.IsNullOrEmpty(user.NewPassword) || dbUser.Username != user.Username)
            {
                var userRabbitMq = _mapper.Map<UserRabbitMq>(user);
                userRabbitMq.Uuid = dbUser.Uuid;
                _publisher.Publish(userRabbitMq, RabbitMqRouting.UpdateUser, RabbitMqExchange.UserExchange);
            }

            await _userDal.Update(dbUser);
        }

        /// <summary>
        /// Checks if the username and email is already in use
        /// </summary>
        /// <param name="user">The updated user data</param>
        /// <param name="dbUser">The found user in the database by uuid</param>
        private async Task CheckForDuplicatedUserData(User user, UserDto dbUser)
        {
            if (user.Email != dbUser.Email && await _userDal.Find(null, user.Email) != null)
            {
                throw new DuplicateNameException();
            }

            if (user.Username != dbUser.Username && await _userDal.Find(user.Username, null) != null)
            {
                throw new DuplicateNameException();
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
                    _publisher.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser, RabbitMqExchange.UserExchange);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.Admin when dbUserToDelete.AccountRole == AccountRole.User:
                    _publisher.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser, RabbitMqExchange.UserExchange);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.User when requestingUser.Uuid == userUuidToDeleteUuid:
                    _publisher.Publish(userUuidToDeleteUuid, RabbitMqRouting.DeleteUser, RabbitMqExchange.UserExchange);
                    await _userDal.Delete(userUuidToDeleteUuid);
                    break;
                case AccountRole.Undefined:
                    break;
                default:
                    throw new UnauthorizedAccessException();
            }
        }
    }
}
