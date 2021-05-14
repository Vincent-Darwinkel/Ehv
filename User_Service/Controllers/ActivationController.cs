using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Service.Logic;

namespace User_Service.Controllers
{
    [Route("user/activate")]
    [ApiController]
    public class ActivationController : ControllerBase
    {
        private readonly ActivationLogic _activationLogic;
        private readonly LogLogic _logLogic;

        public ActivationController(ActivationLogic activationLogic, LogLogic logLogic)
        {
            _activationLogic = activationLogic;
            _logLogic = logLogic;
        }

        [HttpPost]
        public async Task<ActionResult> ActivateUser(string code)
        {
            try
            {
                await _activationLogic.ActivateUser(code);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
