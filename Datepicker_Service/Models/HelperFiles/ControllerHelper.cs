using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Enums;
using Datepicker_Service.Logic;
using Datepicker_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Mvc;
using System;
using RequestHeaders = Datepicker_Service.Models.FromFrontend.RequestHeaders;

namespace Datepicker_Service.Models.HelperFiles
{
    public class ControllerHelper
    {
        private readonly JwtLogic _jwtLogic;

        public ControllerHelper(JwtLogic jwtLogic)
        {
            _jwtLogic = jwtLogic;
        }

        public User GetRequestingUser(ControllerBase controllerBase)
        {
            string jwt = controllerBase.HttpContext.Request.Headers[RequestHeaders.Jwt];
            if (jwt.Length < 25)
            {
                throw new UnprocessableException();
            }

            return new User
            {
                Uuid = _jwtLogic.GetClaim<Guid>(jwt, JwtClaim.Uuid),
                AccountRole = _jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole)
            };
        }
    }
}
