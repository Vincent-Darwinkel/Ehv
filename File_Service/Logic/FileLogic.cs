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
            List<IFormFile> validFiles = await _fileHelper.FilterFiles(files);
            if (validFiles.Count == 0)
            {
                throw new UnprocessableException();
            }

            string fullPath = $"{Environment.CurrentDirectory}/{userSpecifiedPath}";
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                // todo add directory to db
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

            var videoNames = new Dictionary<string, IFormFile>();
            foreach (var video in videoCollection)
            {
                string fileLocation = await CompressVideo(video, fullPath);
                videoNames.Add(fileLocation, video);
            }

            var imageNames = new Dictionary<string, IFormFile>();
            foreach (var image in imageCollection)
            {
                string fileName = await CompressAndSaveImage(image, fullPath);
                imageNames.Add(fileName, image);
            }
        }

        private async Task<string> CompressAndSaveImage(IFormFile image, string path)
        {
            string fileExtension = image.ContentType.Replace("image/", ".");
            string newFileName = Guid.NewGuid().ToString();
            string tempPath = $"{Environment.CurrentDirectory}/Media/TempFiles/{Guid.NewGuid()}/";
            Directory.CreateDirectory(tempPath);
            File.Copy($"{Environment.CurrentDirectory}/Media/TempFiles/ImageConverter.py", $"{tempPath}ImageConverter.py");
            await using (Stream fileStream = new FileStream($"{tempPath}input{fileExtension}", FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            SystemHelper.ExecuteOsCommand($"python3 {tempPath}ImageConverter.py");
            File.Move($"{tempPath}output.webp", $"{path}{newFileName}.webp");
            DeleteDirectory(tempPath);
            return $"{newFileName}.webp";
        }

        private void DeleteDirectory(string fullPath)
        {
            DirectoryInfo di = new DirectoryInfo(fullPath);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            Directory.Delete(fullPath);
        }

        private async Task<string> CompressVideo(IFormFile video, string path)
        {
            string fileExtension = video.ContentType.Replace("video/", ".");
            string tempFileName = Guid.NewGuid().ToString();
            string newFileName = Guid.NewGuid().ToString();
            string tempPath = $"{Environment.CurrentDirectory}/Media/TempFiles/";
            await using (Stream fileStream = new FileStream(tempPath + tempFileName + fileExtension, FileMode.Create))
            {
                await video.CopyToAsync(fileStream);
            }

            SystemHelper.ExecuteOsCommand($"ffmpeg -i {tempPath + tempFileName + fileExtension} -b:a 300k {path}{newFileName}.mp4");
            File.Delete(tempPath + tempFileName + fileExtension);
            return $"{path + newFileName}.mp4";
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