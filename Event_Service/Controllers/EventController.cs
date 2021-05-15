using AutoMapper;
using Event_Service.Logic;
using Event_Service.Models;
using Event_Service.Models.HelperFiles;
using Event_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_Service.Controllers
{
    [Route("event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EventLogic _eventLogic;
        private readonly ControllerHelper _controllerHelper;
        private readonly LogLogic _logLogic;

        public EventController(IMapper mapper, EventLogic eventLogic, ControllerHelper controllerHelper,
            LogLogic logLogic)
        {
            _mapper = mapper;
            _eventLogic = eventLogic;
            _controllerHelper = controllerHelper;
            _logLogic = logLogic;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventViewmodel>>> All()
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                List<EventDto> events = await _eventLogic.All(requestingUser);
                events.ForEach(e =>
                    e.EventDates.ForEach(ed => ed.EventDateUsers
                        .RemoveAll(edu => edu.UserUuid != requestingUser.Uuid)));

                return _mapper.Map<List<EventViewmodel>>(events);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<EventViewmodel>> Find(Guid uuid)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                return await _eventLogic.Find(uuid, requestingUser);
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

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _eventLogic.RemoveAsync(uuid, requestingUser);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}