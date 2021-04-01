using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Logic;
using File_Service.Models.FromFrontend;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace File_Service.Controllers
{
    [Route("directory")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly DirectoryLogic _directoryLogic;

        public DirectoryController(DirectoryLogic directoryLogic)
        {
            _directoryLogic = directoryLogic;
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("info")]
        public async Task<ActionResult<DirectoryInfoFile>> GetDirectoryInfo(string path)
        {
            try
            {
                var userUuid =
                    Guid.Parse("091f31ae-a4e5-41b1-bb86-48dbfe40b839"); // TODO remove this temporary variable
                return await _directoryLogic.GetDirectoryInfo(path, userUuid);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (UnprocessableException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateFolder([FromForm] FolderUpload folder)
        {
            try
            {
                var userUuid =
                    Guid.Parse("091f31ae-a4e5-41b1-bb86-48dbfe40b839"); // TODO remove this temporary variable
                await _directoryLogic.CreateFolder(folder, userUuid);
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}