using AutoMapper;
using Hobby_Service.Logic;
using Hobby_Service.Models;
using Hobby_Service.Models.FromFrontend;
using Hobby_Service.Models.ToFrontend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hobby_Service.CustomExceptions;
using Hobby_Service.Enums;

namespace Hobby_Service.Controllers
{
    [Route("hobby")]
    [ApiController]
    public class HobbyController : ControllerBase
    {
        private readonly HobbyLogic _hobbyLogic;
        private readonly LogLogic _logLogic;
        private readonly IMapper _mapper;

        public HobbyController(HobbyLogic hobbyLogic, LogLogic logLogic, IMapper mapper)
        {
            _hobbyLogic = hobbyLogic;
            _logLogic = logLogic;
            _mapper = mapper;
        }

        [AuthorizedAction(new[] { AccountRole.SiteAdmin })]
        [HttpPost]
        public async Task<ActionResult> Add(string name)
        {
            try
            {
                await _hobbyLogic.Add(new HobbyDto
                {
                    Uuid = Guid.NewGuid(),
                    Name = name
                });
                return Ok();
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
        public async Task<ActionResult<List<HobbyViewmodel>>> All()
        {
            try
            {
                List<HobbyDto> hobbies = await _hobbyLogic.All();
                return _mapper.Map<List<HobbyViewmodel>>(hobbies);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AuthorizedAction(new[] { AccountRole.SiteAdmin })]
        [HttpPut]
        public async Task<ActionResult> Update(Hobby hobby)
        {
            try
            {
                var hobbyDto = _mapper.Map<HobbyDto>(hobby);
                await _hobbyLogic.Update(hobbyDto);
                return Ok();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AuthorizedAction(new[] { AccountRole.SiteAdmin })]
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name = "uuid-collection")] List<Guid> uuidCollection)
        {
            try
            {
                await _hobbyLogic.Delete(uuidCollection);
                return Ok();
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
    }
}