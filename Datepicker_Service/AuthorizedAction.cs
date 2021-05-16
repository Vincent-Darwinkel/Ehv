using Datepicker_Service.Enums;
using Datepicker_Service.Logic;
using Datepicker_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Datepicker_Service
{
    public class AuthorizedAction : ActionFilterAttribute
    {
        private readonly AccountRole[] _requiredRoles;

        public AuthorizedAction(AccountRole[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is

            if (allowAnonymous) // skip authorization if allow anonymous attribute is used
            {
                return;
            }

            JwtLogic jwtLogic = (JwtLogic)context.HttpContext.RequestServices.GetService(typeof(JwtLogic));
            string jwt = context.HttpContext.Request.Headers[RequestHeaders.Jwt];

            var role = jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole);
            if (!_requiredRoles.Contains(role))
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}