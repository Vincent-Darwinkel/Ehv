using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Logging_Service.Logic;
using Logging_Service.Models;
using Logging_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ActionResult<List<Log>>> Find(List<Guid> uuidCollection)
        {
            try
            {
                List<LogDto> logCollection = await _logLogic.Find(uuidCollection);
                return _mapper.Map<List<Log>>(logCollection);
            }
            catch (Exception e)
            {
                await _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<List<Log>>> Find(string microService)
        {
            try
            {
                List<LogDto> logCollection = await _logLogic.Find(microService);
                return _mapper.Map<List<Log>>(logCollection);
            }
            catch (Exception e)
            {
                await _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

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