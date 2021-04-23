using System;
using Datepicker_Service.Enums;
using Datepicker_Service.Logic;
using Datepicker_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
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
            return new User
            {
                Uuid = _jwtLogic.GetClaim<Guid>(jwt, JwtClaim.Uuid),
                AccountRole = _jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole)
            };
        }
    }
}
