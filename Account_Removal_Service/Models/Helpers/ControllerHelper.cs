using Account_Removal_Service.CustomExceptions;
using Account_Removal_Service.Enums;
using Account_Removal_Service.Logic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Account_Removal_Service.Models.Helpers
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
    }
}
