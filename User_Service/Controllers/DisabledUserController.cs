using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Service.Logic;
using User_Service.Models.FromFrontend;

namespace User_Service.Controllers
{
    [Route("user/disabled")]
    [ApiController]
    public class DisabledUserController : ControllerBase
    {
        private readonly DisabledUserLogic _disabledUserLogic;
        private readonly LogLogic _logLogic;

        public DisabledUserController(DisabledUserLogic disabledUserLogic, LogLogic logLogic)
        {
            _disabledUserLogic = disabledUserLogic;
            _logLogic = logLogic;
        }

        public async Task<ActionResult> Add(DisabledUser disabledUser)
        {
            try
            {
                await _disabledUserLogic.Add(disabledUser);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Delete(Guid userUuid)
        {
            try
            {
                await _disabledUserLogic.Delete(userUuid);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
