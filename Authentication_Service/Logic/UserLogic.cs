using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.RabbitMq;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Authentication_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly SecurityLogic _securityLogic;
        private readonly IMapper _mapper;

        public UserLogic(IUserDal userDal, SecurityLogic securityLogic, IMapper mapper)
        {
            _userDal = userDal;
            _securityLogic = securityLogic;
            _mapper = mapper;
        }

        private bool UserModelValid(UserDto user)
        {
            return user.Uuid != Guid.Empty &&
                   !string.IsNullOrEmpty(user.Username) &&
                   !string.IsNullOrEmpty(user.Password) &&
                   user.AccountRole != AccountRole.Undefined;
        }

        public async Task Add(UserRabbitMqSensitiveInformation user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            if (!UserModelValid(userDto))
            {
                throw new UnprocessableException();
            }

            userDto.Password = _securityLogic.HashPassword(user.Password);
            await _userDal.Add(userDto);
        }

        public async Task<string> ValidateUserPassword(string user)
        {
            var userRabbitMq = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDto>(user);
            UserDto dbUser = await _userDal.Find(userRabbitMq.Username);

            bool passwordCorrect = _securityLogic.VerifyPassword(userRabbitMq.Password, dbUser.Password);
            return Newtonsoft.Json.JsonConvert.SerializeObject(passwordCorrect);
        }

        public async Task Update(UserDto user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            UserDto dbUser = await _userDal.Find(user.Uuid);
            dbUser.Username = user.Username;
            if (!_securityLogic.VerifyPassword(user.Password, dbUser.Password))
            {
                dbUser.Password = _securityLogic.HashPassword(user.Password);
            }

            await _userDal.Update(dbUser);
        }

        public async Task Delete(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            await _userDal.Delete(uuid);
        }
    }
}
