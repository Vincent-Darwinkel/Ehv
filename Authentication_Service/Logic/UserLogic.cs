using Authentication_Service.CustomExceptions;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Enums;
using Authentication_Service.Models.Dto;
using System;
using System.Threading.Tasks;

namespace Authentication_Service.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly SecurityLogic _securityLogic;

        public UserLogic(IUserDal userDal, SecurityLogic securityLogic)
        {
            _userDal = userDal;
            _securityLogic = securityLogic;
        }

        private bool UserModelValid(UserDto user)
        {
            return user.Uuid != Guid.Empty &&
                   !string.IsNullOrEmpty(user.Username) &&
                   !string.IsNullOrEmpty(user.Password) &&
                   user.AccountRole != AccountRole.Undefined;
        }

        public async Task Add(UserDto user)
        {
            if (!UserModelValid(user))
            {
                throw new UnprocessableException();
            }

            user.Password = _securityLogic.HashPassword(user.Password);
            await _userDal.Add(user);
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
