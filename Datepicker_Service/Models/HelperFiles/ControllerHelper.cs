using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Enums;
using Datepicker_Service.Logic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Datepicker_Service.Models.HelperFiles
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
            string authorization = controllerBase.HttpContext.Request.Headers[RequestHeaders.Jwt];
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
