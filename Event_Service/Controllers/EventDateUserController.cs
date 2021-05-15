using System;
using System.Data;
using System.Threading.Tasks;
using Event_Service.Enums;
using Event_Service.Logic;
using Event_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_Service.Controllers
{
    [Route("event/date/user")]
    [ApiController]
    public class EventDateUserController : ControllerBase
    {
        private readonly EventDateUserLogic _eventDateUserLogic;
        private readonly ControllerHelper _controllerHelper;
        private readonly LogLogic _logLogic;

        public EventDateUserController(EventDateUserLogic eventDateUserLogic,
            ControllerHelper controllerHelper, LogLogic logLogic)
        {
            _eventDateUserLogic = eventDateUserLogic;
            _controllerHelper = controllerHelper;
            _logLogic = logLogic;
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> UnsubscribeFromEventDate(Guid uuid)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _eventDateUserLogic.Remove(uuid, requestingUser);
                return Ok();
            }
            catch (NoNullAllowedException)
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
