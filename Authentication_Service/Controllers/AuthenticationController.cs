using Authentication_Service.CustomExceptions;
using Authentication_Service.Enums;
using Authentication_Service.Logic;
using Authentication_Service.Models.Dto;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.HelperFiles;
using Authentication_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Authentication_Service.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationLogic _authorizationLogic;
        private readonly LogLogic _logLogic;
        private readonly JwtLogic _jwtLogic;
        private readonly ControllerHelper _controllerHelper;

        public AuthenticationController(AuthenticationLogic authorizationLogic, LogLogic logLogic, JwtLogic jwtLogic,
            ControllerHelper controllerHelper)
        {
            _authorizationLogic = authorizationLogic;
            _logLogic = logLogic;
            _jwtLogic = jwtLogic;
            _controllerHelper = controllerHelper;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            try
            {
                LoginResultViewmodel result = await _authorizationLogic.Login(login);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (DisabledUserException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthorizationTokensViewmodel>> RefreshJwt(Guid refreshToken)
        {
            try
            {
                UserDto requestingUser = _controllerHelper.GetRequestingUser(this);
                string jwt = _controllerHelper.GetJwt(this);
                return await _jwtLogic.RefreshJwt(jwt, refreshToken, requestingUser);
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
