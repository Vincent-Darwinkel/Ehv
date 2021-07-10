using File_Service.CustomExceptions;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace File_Service.Logic
{
    public class FileLogic
    {
        private readonly FileHelper _fileHelper;

        public FileLogic(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        /// <summary>
        /// Finds the file by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the file to find</param>
        /// <returns>A FileContentResult which contains the file</returns>
        public async Task<FileContentResult> Find(Guid uuid)
        {
            string foundFilePath = FileHelper.GetFilePathByUuid(uuid);
            if (string.IsNullOrEmpty(foundFilePath))
            {
                throw new FileNotFoundException();
            }

            var fileInfo = new FileInfo(foundFilePath);
            byte[] fileBytes = await File.ReadAllBytesAsync(foundFilePath);
            return new FileContentResult(fileBytes, fileInfo.Extension == ".webp" ? "image/webp" : "video/mp4");
        }

        /// <summary>
        /// Saves the file on the file system if the provided userSpecifiedPath is valid, the file is an webp image or mp4 video and the file does not contain viruses
        /// </summary>
        /// <param name="files">The files to save</param>
        /// <param name="userSpecifiedPath">The userSpecifiedPath to save the files in</param>
        /// <param name="requestingUserUuid">The uuid of the requesting user</param>
        /// <returns>A list of the name of the files that are saved</returns>
        public async Task SaveFile(List<IFormFile> files, string userSpecifiedPath, Guid requestingUserUuid)
        {
            string fullPath = $"{Environment.CurrentDirectory}/Media{userSpecifiedPath}";
            List<IFormFile> validFiles = await _fileHelper.FilterFiles(files);
            if (validFiles.Count == 0)
            {
                throw new UnprocessableException();
            }

            string[] supportedImageFileTypes = { ".webp", ".png", ".jpeg", ".jpg" };
            List<IFormFile> imageCollection = validFiles
                .FindAll(file => supportedImageFileTypes
                .Any(sift => file.FileName
                    .EndsWith(sift)));

            string[] supportedVideoFileTypes = { ".webm", ".mp4", ".mov", ".avi" };
            List<IFormFile> videoCollection = validFiles
                .FindAll(file => supportedVideoFileTypes
                    .Any(sift => file.FileName
                        .EndsWith(sift)));
            foreach (var video in videoCollection)
            {
                await CompressVideo(video, fullPath);
            }
        }

        private async Task CompressVideo(IFormFile video, string path)
        {
            string fileExtension = video.ContentType.Replace("video/", ".");
            string tempFileName = Guid.NewGuid().ToString();
            string newFileName = Guid.NewGuid().ToString();
            string tempPath = $"{Environment.CurrentDirectory}/Media/TempFiles/";
            await using (Stream fileStream = new FileStream(tempPath + tempFileName + fileExtension, FileMode.Create))
            {
                await video.CopyToAsync(fileStream);
            }

            SystemHelper.ExecuteOsCommand($"ffmpeg -i {tempPath + tempFileName + fileExtension} -b:a 800k {tempPath + newFileName}.mp4");
            File.Delete(tempPath + tempFileName + fileExtension);
        }

        /// <summary>
        /// Removes a file by uuid if the user is owner and the file exists
        /// </summary>
        /// <param name="fileUuid">The uuid of the file to remove</param>
        /// <param name="requestingUser">The user that made the request</param>
        public async Task Delete(Guid fileUuid, UserHelper requestingUser)
        {
            if (fileUuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            string directoryPath = FileHelper.GetDirectoryPathByFileUuid(fileUuid);
            string fullPath = FileHelper.GetFilePathByUuid(fileUuid);

            if (!File.Exists(fullPath))
            {
                throw new UnprocessableException();
            }
        }
    }
}