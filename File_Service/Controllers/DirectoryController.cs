using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Logic;
using File_Service.Models.FromFrontend;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace File_Service.Controllers
{
    [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
    [Route("directory")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly DirectoryLogic _directoryLogic;
        private readonly LogLogic _logLogic;
        private readonly ControllerHelper _controllerHelper;

        public DirectoryController(DirectoryLogic directoryLogic, LogLogic logLogic, ControllerHelper controllerHelper)
        {
            _directoryLogic = directoryLogic;
            _logLogic = logLogic;
            _controllerHelper = controllerHelper;
        }

        [HttpGet]
        public ActionResult<List<string>> GetItemsInFolder(string path)
        {
            try
            {
                return _directoryLogic.GetItems(path);
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

        [HttpGet("info")]
        public async Task<ActionResult<DirectoryInfoFile>> GetDirectoryInfo(string path)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                return await _directoryLogic.GetDirectoryInfo(path, requestingUser.Uuid);
            }
            catch (FileNotFoundException)
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

        [HttpPost]
        public async Task<ActionResult> CreateFolder([FromForm] FolderUpload folder)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _directoryLogic.CreateFolder(folder, requestingUser.Uuid);
                return Ok();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
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

        [HttpDelete]
        public async Task<ActionResult> RemoveDirectory(string path)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _directoryLogic.RemoveDirectory(path, requestingUser);
                return Ok();
            }
            catch (UnprocessableException)
            {
                return UnprocessableEntity();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logLogic.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}