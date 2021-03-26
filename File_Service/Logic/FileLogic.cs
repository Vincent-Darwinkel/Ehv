using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace File_Service.Logic
{
    public class FileLogic
    {
        private readonly FileHelper _fileHelper;
        private readonly ConcurrentBag<string> _savedFiles = new ConcurrentBag<string>();

        public FileLogic(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public async Task<FileContentResult> GetFileAsync(Guid uuid)
        {
            string foundFilePath = DirectoryHelper.GetFilePathByUuid(uuid);
            if (string.IsNullOrEmpty(foundFilePath))
            {
                throw new FileNotFoundException();
            }

            var fileInfo = new FileInfo(foundFilePath);
            byte[] fileBytes = await File.ReadAllBytesAsync(foundFilePath);
            return new FileContentResult(fileBytes, fileInfo.Extension == ".webp" ? "image/webp" : "video/mp4");
        }

        /// <summary>
        /// Adds / to the start and or end of the path if it does not exists
        /// </summary>
        /// <param name="path">The path to check and fix</param>
        /// <returns>A path with / at beginning and end</returns>
        private string FixPath(string path)
        {
            if (!path.EndsWith("/"))
            {
                path += "/";
            }
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            return path;
        }

        /// <summary>
        /// Saves the file on the file system if the provided path is valid, the file is an webp image or mp4 video and the file does not contain viruses
        /// </summary>
        /// <param name="files">The files to save</param>
        /// <param name="path">The path to save the files in</param>
        /// <param name="requestingUserUuid">The uuid of the requesting user</param>
        /// <returns>A list of the name of the files that are saved</returns>
        public async Task<List<string>> SaveFileAsync(List<IFormFile> files, string path, Guid requestingUserUuid)
        {
            path = FixPath(path);
            if (files == null || files.Count == 0 || !DirectoryHelper.PathIsValid(path))
            {
                throw new UnprocessableException();
            }

            string userDirectory = $"{Environment.CurrentDirectory}/Media{path}{requestingUserUuid}/";
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            List<IFormFile> validFiles = await _fileHelper.FilterFiles(files);
            var fileTasks = validFiles.Select(file =>
                _fileHelper.GetFileTypeFromFile(file) == FileType.Image
                    ? SaveImageAsync(userDirectory, file)
                    : SaveVideoAsync(userDirectory, file));

            await Task.WhenAll(fileTasks);
            return _savedFiles.ToList();
        }

        private async Task SaveVideoAsync(string path, IFormFile video)
        {
            await using var ms = new MemoryStream();
            try
            {
                await video.OpenReadStream().CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();
                string filePath = path + Guid.NewGuid() + _fileHelper.GetFileExtensionFromFile(video);
                await File.WriteAllBytesAsync(filePath, fileBytes);
                _savedFiles.Add(video.FileName);
            }
            finally
            {
                ms.Close();
            }
        }

        private async Task SaveImageAsync(string path, IFormFile image)
        {
            await using var ms = new MemoryStream();
            try
            {
                await image.OpenReadStream().CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();
                string filePath = path + Guid.NewGuid() + _fileHelper.GetFileExtensionFromFile(image);
                await File.WriteAllBytesAsync(filePath, fileBytes);
                _savedFiles.Add(image.FileName);
            }
            finally
            {
                ms.Close();
            }
        }
    }
}