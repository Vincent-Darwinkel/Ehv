using AutoMapper;
using Datepicker_Service.Logic;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Datepicker_Service.Enums;

namespace Datepicker_Service.Controllers
{
    [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin })]
    [Route("datepicker/availability")]
    [ApiController]
    public class DatepickerAvailabilityController : ControllerBase
    {
        private readonly DatepickerAvailabilityLogic _datepickerAvailabilityLogic;
        private readonly ControllerHelper _controllerHelper;
        private readonly IMapper _mapper;
        private readonly LogLogic _logLogic;

        public DatepickerAvailabilityController(DatepickerAvailabilityLogic datepickerAvailabilityLogic,
            ControllerHelper controllerHelper, IMapper mapper, LogLogic logLogic)
        {
            _datepickerAvailabilityLogic = datepickerAvailabilityLogic;
            _controllerHelper = controllerHelper;
            _mapper = mapper;
            _logLogic = logLogic;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] UserAvailability availability)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _datepickerAvailabilityLogic.AddOrUpdateAsync(availability.AvailableDates, availability.DatepickerUuid, requestingUser);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status304NotModified);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
