using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.RabbitMq;
using Authentication_Service.RabbitMq.Publishers;
using Authentication_Service.RabbitMq.Rpc;
using AutoMapper;

namespace Authentication_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly SecurityLogic _securityLogic;
        private readonly IPublisher _publisher;
        private readonly RpcClient _rpcClient;
        private readonly IMapper _mapper;

        public UserLogic(IUserDal userDal, SecurityLogic securityLogic, IPublisher publisher,
            RpcClient rpcClient, IMapper mapper)
        {
            _userDal = userDal;
            _securityLogic = securityLogic;
            _publisher = publisher;
            _rpcClient = rpcClient;
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

            user.Password = _securityLogic.HashPassword(user.Password);
            await _userDal.Add(userDto);
        }

        public async Task Update(UserDto user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            UserDto dbUser = await _userDal.Find(user.Uuid);
            dbUser.Username = user.Username;
            dbUser.AccountRole = user.AccountRole;
            if (user.Password != dbUser.Password)
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
