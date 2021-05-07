using AutoMapper;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Logic;
using Datepicker_Service.Models;
using Datepicker_Service.Models.FromFrontend;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datepicker_Service.Controllers
{
    [Route("datepicker")]
    [ApiController]
    public class DatepickerController : ControllerBase
    {
        private readonly DatepickerLogic _datepickerLogic;
        private readonly IMapper _mapper;
        private readonly ControllerHelper _controllerHelper;
        private readonly LogLogic _logLogic;

        public DatepickerController(DatepickerLogic datepickerLogic, IMapper mapper,
            ControllerHelper controllerHelper, LogLogic logLogic)
        {
            _datepickerLogic = datepickerLogic;
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
                await _datepickerLogic.Add(datepickerDto, _controllerHelper.GetRequestingUser(this));
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
                User requestingUser = _controllerHelper.GetRequestingUser(this);
                DatepickerDto datepicker = await _datepickerLogic.Find(uuid);
                var datepickerViewmodel = _mapper.Map<DatepickerViewmodel>(datepicker);
                datepickerViewmodel.CanBeRemoved = datepicker.AuthorUuid == requestingUser.Uuid;

                return datepickerViewmodel;
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
                await _datepickerLogic.Update(datepickerDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnprocessableException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity);
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
                Guid userUuid = _controllerHelper.GetRequestingUser(this).Uuid;
                await _datepickerLogic.Delete(uuid, userUuid);
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
