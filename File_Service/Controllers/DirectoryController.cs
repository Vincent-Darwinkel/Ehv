using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Logic;
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

        [HttpPost]
        public async Task<ActionResult> CreateFolder(string path)
        {
            try
            {
                var userUuid =
                    Guid.Parse("091f31ae-a4e5-41b1-bb86-48dbfe40b839"); // TODO remove this temporary variable
                await _directoryLogic.CreateFolder(path, userUuid);
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}