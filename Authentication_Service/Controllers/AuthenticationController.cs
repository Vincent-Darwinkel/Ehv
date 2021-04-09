using System;
using System.Threading.Tasks;
using Authentication_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Service.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Login([FromForm] Login login)
        {
            try
            {
                return Ok();
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
