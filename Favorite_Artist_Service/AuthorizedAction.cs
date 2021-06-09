using Favorite_Artist_Service.Enums;
using Favorite_Artist_Service.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using RequestHeaders = Favorite_Artist_Service.Model.Helpers.RequestHeaders;

namespace Favorite_Artist_Service
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
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (allowAnonymous) // skip authorization if allow anonymous attribute is used
            {
                return;
            }

            JwtLogic jwtLogic = (JwtLogic)context.HttpContext.RequestServices.GetService(typeof(JwtLogic));
            string authorization = context.HttpContext.Request.Headers[RequestHeaders.Authorization];
            if (string.IsNullOrEmpty(authorization))
            {
                context.Result = new UnauthorizedResult();
                base.OnActionExecuting(context);
                return;
            }

            string jwt = authorization.Replace("Bearer ", "");

            var role = jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole);
            if (!_requiredRoles.Contains(role))
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}