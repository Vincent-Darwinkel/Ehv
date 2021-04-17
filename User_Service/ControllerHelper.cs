using System;
using Microsoft.AspNetCore.Mvc;
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
            return new UserDto
            {
                Uuid = _jwtLogic.GetClaim<Guid>(jwt, JwtClaim.UserUuid),
                Username = _jwtLogic.GetClaim<string>(jwt, JwtClaim.Username),
                AccountRole = _jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole)
            };
        }
    }
}
