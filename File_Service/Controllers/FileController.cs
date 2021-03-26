using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Logic;

namespace File_Service.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileLogic _imageLogic;

        public FileController(FileLogic imageLogic)
        {
            _imageLogic = imageLogic;
        }

        [HttpGet("{uuid}")]
        public async Task<FileContentResult> GetFileByUuidAsync(Guid uuid)
        {
            return await _imageLogic.FindAsync(uuid);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFilesAsync([FromForm] List<IFormFile> files, string path)
        {
            try
            {
                var userUuid =
                    Guid.Parse("091f31ae-a4e5-41b1-bb86-48dbfe40b839"); // TODO remove this temporary variable
                await _imageLogic.SaveFileAsync(files, path, userUuid);
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
