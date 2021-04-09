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
        private readonly SecurityLogic _securityLogic;
        private readonly JwtLogic _jwtLogic;

        public AuthenticationLogic(IUserDal userDal, SecurityLogic securityLogic, JwtLogic jwtLogic)
        {
            _userDal = userDal;
            _securityLogic = securityLogic;
            _jwtLogic = jwtLogic;
        }

        /// <summary>
        /// Checks if the credentials are correct and returns an jwt and refresh token if password is correct
        /// </summary>
        /// <param name="login">The username and password</param>
        /// <returns>An jwt and refresh token if password is correct, if not correct null is returned</returns>
        public async Task<LoginResultViewmodel> Login(Login login)
        {
            UserDto dbUser = await _userDal.Find(login.Username);
            if (dbUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            bool passwordCorrect = _securityLogic.VerifyPassword(login.Password, dbUser.Password);
            if (passwordCorrect)
            {
                return await _jwtLogic.CreateJwt(dbUser);
            }

            throw new UnauthorizedAccessException();
        }
    }
}