using AutoMapper;
using Logging_Service.Enums;
using Logging_Service.Logic;
using Logging_Service.Models;
using Logging_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging_Service.Controllers
{
    [AuthorizedAction(new[] { AccountRole.SiteAdmin })]
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

        [HttpGet]
        public async Task<ActionResult<List<LogViewmodel>>> All()
        {

            List<LogDto> logCollection = await _logLogic.All();
            return _mapper.Map<List<LogViewmodel>>(logCollection);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name = "uuid-collection")] Guid[] uuidCollection)
        {
            try
            {
                await _logLogic.Delete(uuidCollection.ToList());
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