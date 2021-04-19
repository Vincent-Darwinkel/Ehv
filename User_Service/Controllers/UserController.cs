using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Service.CustomExceptions;
using User_Service.Logic;
using User_Service.Models;
using User_Service.Models.FromFrontend;
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

        public UserController(UserLogic userLogic, IMapper mapper, ControllerHelper helper)
        {
            _userLogic = userLogic;
            _mapper = mapper;
            _helper = helper;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] User user)
        {
            try
            {
                await _userLogic.Register(user);
                return Ok();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }
            catch (UnprocessableException)
            {
                return UnprocessableEntity();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> Find([FromBody] List<Guid> uuidCollection)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromForm] User user)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{uuid}")]
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}