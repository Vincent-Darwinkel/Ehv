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
    [Route("video")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly FileLogic _fileLogic;

        public VideoController(FileLogic fileLogic)
        {
            _fileLogic = fileLogic;
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult> GetFileByUuidAsync(Guid uuid)
        {
            try
            {
                byte[] fileBytes = await _fileLogic.GetFileAsync(uuid, FileType.Video);
                return File(fileBytes, "video/mp4");
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
                List<string> savedFiles = await _fileLogic.SaveFileAsync(data, userUuid, FileType.Video);
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