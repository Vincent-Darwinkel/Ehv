using Event_Service.CustomExceptions;
using Event_Service.Enums;
using Event_Service.Logic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Event_Service.Models.HelperFiles
{
    public class ControllerHelper
    {
        private readonly JwtLogic _jwtLogic;

        public ControllerHelper(JwtLogic jwtLogic)
        {
            _jwtLogic = jwtLogic;
        }

        public UserHelper GetRequestingUser(ControllerBase controllerBase)
        {
            string authorization = controllerBase.HttpContext.Request.Headers[RequestHeaders.Authorization];
            string jwt = authorization.Replace("Bearer ", "");
            if (jwt.Length < 25)
            {
                throw new UnprocessableException();
            }

            return new UserHelper
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
