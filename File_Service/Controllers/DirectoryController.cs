using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Logic;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using File_Service.Models;
using File_Service.Models.ToFrontend;

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
        public async Task<ActionResult<List<FileInfo>>> GetItemsInFolder(string path)
        {
            UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
            try
            {
                List<FileDto> fileUuidCollection = await _directoryLogic.GetFileNamesInDirectory(path);
                List<DirectoryDto> directoryUuidCollection = await _directoryLogic.GetFoldersInDirectory(path);

                var items = new List<FileInfo>();
                fileUuidCollection.ForEach(f => items.Add(new FileInfo
                {
                    Uuid = f.Uuid,
                    RequestingUserIsOwner = f.OwnerUuid == requestingUser.Uuid,
                    FileType = f.FileType.ToString()
                }));

                directoryUuidCollection.ForEach(d => items.Add(new FileInfo
                {
                    Uuid = d.Uuid,
                    RequestingUserIsOwner = d.OwnerUuid == requestingUser.Uuid,
                    IsDirectory = true,
                    Name = d.Name
                }));

                return items;
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

        [HttpPost]
        public async Task<ActionResult> CreateFolder(string path)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _directoryLogic.CreateDirectory(path, requestingUser.Uuid);
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

        [HttpPut]
        public async Task<ActionResult> RenameDirectory(Guid uuid, string name)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _directoryLogic.RenameDirectory(uuid, requestingUser, name);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> RemoveDirectory(Guid uuid)
        {
            try
            {
                UserHelper requestingUser = _controllerHelper.GetRequestingUser(this);
                await _directoryLogic.Delete(uuid, requestingUser);
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