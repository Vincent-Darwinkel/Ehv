using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Logic;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Datepicker_Service.Controllers
{
    [Route("datepicker")]
    [ApiController]
    public class DatepickerController : ControllerBase
    {
        private readonly DatepickerLogic _datepickerlogic;
        private readonly IMapper _mapper;
        private readonly ControllerHelper _controllerHelper;
        private readonly LogLogic _logLogic;

        public DatepickerController(DatepickerLogic datepickerlogic, IMapper mapper,
            ControllerHelper controllerHelper, LogLogic logLogic)
        {
            _datepickerlogic = datepickerlogic;
            _mapper = mapper;
            _controllerHelper = controllerHelper;
            _logLogic = logLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Datepicker datepicker)
        {
            try
            {
                var datepickerDto = _mapper.Map<DatepickerDto>(datepicker);
                await _datepickerlogic.Add(datepickerDto, _controllerHelper.GetRequestingUser(this));
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<DatepickerViewmodel>> Find(Guid uuid)
        {
            try
            {
                DatepickerDto datepicker = await _datepickerlogic.Find(uuid);
                return _mapper.Map<DatepickerViewmodel>(datepicker);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnprocessableException)
            {
                return UnprocessableEntity();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Datepicker datepicker)
        {
            try
            {
                var datepickerDto = _mapper.Map<DatepickerDto>(datepicker);
                await _datepickerlogic.Update(datepickerDto);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Delete(Guid uuid)
        {
            try
            {
                await _datepickerlogic.Delete(uuid);
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
