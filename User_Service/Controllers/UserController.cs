using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models.FromFrontend;

namespace User_Service.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic _userLogic;

        public UserController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromForm] User user)
        {
            try
            {
                await _userLogic.Register(user);
                return Ok();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }
            catch (UnprocessableException)
            {
                return UnprocessableEntity();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
