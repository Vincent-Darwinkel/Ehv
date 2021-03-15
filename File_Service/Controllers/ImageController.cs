using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Logic;
using File_Service.Models.FromFrontend;

namespace File_Service.Controllers
{
    [Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly FileLogic _imageLogic;
        public ImageController(FileLogic imageLogic)
        {
            _imageLogic = imageLogic;
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult> GetFileByUuidAsync(Guid uuid)
        {
            try
            {
                byte[] fileBytes = await _imageLogic.GetFileAsync(uuid, FileType.Image);
                return File(fileBytes, "image/webp");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveFilesAsync([FromForm] FileUpload data, string path)
        {
            try
            {
                var userUuid = Guid.Parse("091f31ae-a4e5-41b1-bb86-48dbfe40b839");
                List<string> savedFiles = await _imageLogic.SaveFileAsync(data, userUuid, FileType.Image);
                return Ok(savedFiles);
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
