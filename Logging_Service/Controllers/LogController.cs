using AutoMapper;
using Logging_Service.Logic;
using Logging_Service.Models;
using Logging_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging_Service.Controllers
{
    [Route("log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogLogic _logLogic;
        private readonly IMapper _mapper;

        public LogController(LogLogic logLogic, IMapper mapper)
        {
            _logLogic = logLogic;
            _mapper = mapper;
        }

        [HttpGet("{microservice}")]
        public async Task<ActionResult<List<LogViewmodel>>> Find(string microService)
        {
            try
            {
                List<LogDto> logCollection = await _logLogic.Find(microService);
                return _mapper.Map<List<LogViewmodel>>(logCollection);
            }
            catch (Exception e)
            {
                await _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<LogViewmodel>>> All()
        {
            try
            {
                List<LogDto> logCollection = await _logLogic.All();
                return _mapper.Map<List<LogViewmodel>>(logCollection);
            }
            catch (Exception e)
            {
                await _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(List<Guid> uuidCollection)
        {
            try
            {
                await _logLogic.Delete(uuidCollection);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                await _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}