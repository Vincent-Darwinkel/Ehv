using System;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.ToFrontend;

namespace Authentication_Service.Logic
{
    public class AuthenticationLogic
    {
        private readonly IUserDal _userDal;

        public AuthenticationLogic(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<LoginResultViewmodel> Login(Login login)
        {
            UserDto dbUser = await _userDal.Find(login.Username);
            if (dbUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            return null;
        }
    }
}