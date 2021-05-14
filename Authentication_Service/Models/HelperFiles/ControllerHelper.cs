using System;
using Authentication_Service.CustomExceptions;
using Authentication_Service.Enums;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Service.Models.HelperFiles
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
            string authorization = controllerBase.HttpContext.Request.Headers[RequestHeaders.Authorization];
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

        public string GetJwt(ControllerBase controllerBase)
        {
            string authorization = controllerBase.HttpContext.Request.Headers[RequestHeaders.Authorization];
            return authorization.Replace("Bearer ", "");
        }
    }
}
