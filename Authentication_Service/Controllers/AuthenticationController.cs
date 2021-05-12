using System;
using System.Threading.Tasks;
using Authentication_Service.CustomExceptions;
using Authentication_Service.Logic;
using Authentication_Service.Models.FromFrontend;
using Authentication_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Service.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationLogic _authorizationLogic;
        private readonly LogLogic _logLogic;

        public AuthenticationController(AuthenticationLogic authorizationLogic, LogLogic logLogic)
        {
            _authorizationLogic = authorizationLogic;
            _logLogic = logLogic;
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
    }
}
