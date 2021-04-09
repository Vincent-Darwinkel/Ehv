using System;
using System.Threading.Tasks;
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

        public AuthenticationController(AuthenticationLogic authorizationLogic)
        {
            _authorizationLogic = authorizationLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm] Login login)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
