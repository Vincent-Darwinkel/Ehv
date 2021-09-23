using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Logic;
using File_Service.Models.FromFrontend;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace File_Service.Controllers
{
    [AuthorizedAction(new[] { AccountRole.User, AccountRole.Admin, AccountRole.SiteAdmin })]
    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileLogic _fileLogic;
        private readonly LogLogic _logLogic;
        private readonly ControllerHelper _controllerHelper;

        public FileController(FileLogic fileLogic, LogLogic logLogic, ControllerHelper controllerHelper)
        {
            _fileLogic = fileLogic;
            _logLogic = logLogic;
            _controllerHelper = controllerHelper;
        }

        [HttpGet("{uuid}")]
        public async Task<FileContentResult> GetFileByUuidAsync(Guid uuid)
        {
            return await _fileLogic.Find(uuid);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFilesAsync([FromForm] FileUpload fileUpload)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _fileLogic.SaveFile(fileUpload.Files, fileUpload.Path, requestingUser.Uuid);
                return Ok();
            }
            catch (DirectoryNotFoundException)
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
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _fileLogic.Delete(uuid, requestingUser);
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
