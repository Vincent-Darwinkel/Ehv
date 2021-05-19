using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using User_Service.CustomExceptions;
using User_Service.Enums;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.FromFrontend;
using User_Service.Models.HelperFiles;
using User_Service.Models.ToFrontend;

namespace User_Service.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic _userLogic;
        private readonly IMapper _mapper;
        private readonly ControllerHelper _helper;
        private readonly LogLogic _logLogic;

        public UserController(UserLogic userLogic, IMapper mapper, ControllerHelper helper,
            LogLogic logLogic)
        {
            _userLogic = userLogic;
            _mapper = mapper;
            _helper = helper;
            _logLogic = logLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] User user)
        {
            try
            {
                await _userLogic.Register(user);
                return Ok();
            }
            catch (AlreadyClosedException)
            {
                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (DuplicateNameException)
            {
                return Conflict();
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

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpGet("{uuid}")]
        public async Task<ActionResult<UserViewModel>> Find(Guid uuid)
        {
            try
            {
                UserDto user = await _userLogic.Find(uuid);
                return _mapper.Map<UserViewModel>(user);
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

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpGet("by-list")]
        public async Task<ActionResult<List<UserViewModel>>> Find([FromQuery(Name = "uuid-collection")] List<Guid> uuidCollection)
        {
            try
            {
                List<UserDto> users = await _userLogic.Find(uuidCollection);
                return _mapper.Map<List<UserViewModel>>(users);
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

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> All()
        {
            try
            {
                List<UserDto> users = await _userLogic.All();
                return _mapper.Map<List<UserViewModel>>(users);
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            try
            {
                UserDto requestingUser = _helper.GetRequestingUser(this);
                await _userLogic.Update(user, requestingUser.Uuid);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (DuplicateNameException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
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

        [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid uuid)
        {
            try
            {
                UserDto requestingUser = _helper.GetRequestingUser(this);
                await _userLogic.Delete(requestingUser, uuid);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (SiteAdminRequiredException)
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
