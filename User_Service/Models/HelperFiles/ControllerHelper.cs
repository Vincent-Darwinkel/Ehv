using Microsoft.AspNetCore.Mvc;
using System;
using User_Service.CustomExceptions;
using User_Service.Enums;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.HelperFiles;

namespace User_Service
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
            string jwt = controllerBase.HttpContext.Request.Headers[RequestHeaders.Jwt];
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
