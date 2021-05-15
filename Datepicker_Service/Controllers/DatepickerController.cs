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
using System.Data;
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
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _datepickerLogic.Add(datepickerDto, requestingUser);
                return Ok();
            }
            catch (DuplicateNameException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("convert")]
        public async Task<ActionResult> Convert([FromBody] DatePickerConversion datepickerConverter)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _datepickerLogic.ConvertDatepicker(datepickerConverter, requestingUser);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (UnprocessableException)
            {
                return UnprocessableEntity();
            }
            catch (NoNullAllowedException)
            {
                return StatusCode(StatusCodes.Status304NotModified);
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
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
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

        [HttpGet]
        public async Task<ActionResult> All()
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                List<DatepickerDto> datePickers = await _datepickerLogic.All();
                var mappedDatepickers = _mapper.Map<List<DatepickerViewmodel>>(datePickers);
                mappedDatepickers.ForEach(mdp =>
                {
                    var dbDatepicker = datePickers.Find(ddp => ddp.Uuid == mdp.Uuid);
                    mdp.CanBeRemoved = requestingUser.Uuid == dbDatepicker.AuthorUuid;
                });

                return Ok(mappedDatepickers);
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
            catch (DuplicateNameException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
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
