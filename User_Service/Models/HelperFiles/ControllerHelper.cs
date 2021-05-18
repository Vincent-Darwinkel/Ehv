using Microsoft.AspNetCore.Mvc;
using System;
using User_Service.CustomExceptions;
using User_Service.Enums;
using User_Service.Logic;

namespace User_Service.Models.HelperFiles
{
    public class ControllerHelper
    {
        private readonly JwtLogic _jwtLogic;

        public ControllerHelper(JwtLogic jwtLogic)
        {
            _jwtLogic = jwtLogic;
        }

        public UserDto GetRequestingUser(ControllerBase controllerBase)
        {
            string authorization = controllerBase.HttpContext.Request.Headers[RequestHeaders.Jwt];
            string jwt = authorization.Replace("Bearer ", "");

            if (jwt.Length < 25)
            {
                throw new UnprocessableException();
            }

            return new UserDto
            {
                Uuid = _jwtLogic.GetClaim<Guid>(jwt, JwtClaim.Uuid),
                AccountRole = _jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole)
            };
        }
    }
}
