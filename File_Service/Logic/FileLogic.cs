using System;
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

        public FileLogic(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        /// <summary>
        /// Finds the file by uuid
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>A FileContentResult which contains the file</returns>
        public async Task<FileContentResult> FindAsync(Guid uuid)
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
        /// Adds / to the start and or end of the userSpecifiedPath if it does not exists
        /// </summary>
        /// <param name="path">The userSpecifiedPath to check and fix</param>
        /// <returns>A userSpecifiedPath with / at beginning and end</returns>
        private string FixPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
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
        /// Saves the file on the file system if the provided userSpecifiedPath is valid, the file is an webp image or mp4 video and the file does not contain viruses
        /// </summary>
        /// <param name="files">The files to save</param>
        /// <param name="userSpecifiedPath">The userSpecifiedPath to save the files in</param>
        /// <param name="requestingUserUuid">The uuid of the requesting user</param>
        /// <returns>A list of the name of the files that are saved</returns>
        public async Task SaveFileAsync(List<IFormFile> files, string userSpecifiedPath, Guid requestingUserUuid)
        {
            userSpecifiedPath = FixPath(userSpecifiedPath);
            if (files?.Count == 0 ||
                !DirectoryHelper.PathIsValid(userSpecifiedPath) ||
                !DirectoryHelper.CanUploadInDirectory(userSpecifiedPath))
            {
                throw new UnprocessableException();
            }
            if (!Directory.Exists($"{Environment.CurrentDirectory}/Media/{userSpecifiedPath}"))
            {
                throw new DirectoryNotFoundException();
            }

            string fullPath = $"{Environment.CurrentDirectory}/Media{userSpecifiedPath}";

            List<IFormFile> validFiles = await _fileHelper.FilterFiles(files);
            var fileNameCollection = new List<string>();
            validFiles.ForEach(file => fileNameCollection.Add(Guid.NewGuid().ToString()));

            var fileTasks = validFiles.Select((file, index) =>
                _fileHelper.GetFileTypeFromFile(file) == FileType.Image
                    ? SaveImageAsync($"{fullPath}/{fileNameCollection[index]}{_fileHelper.GetExtension(file)}", file)

                    : SaveVideoAsync($"{fullPath}/{fileNameCollection[index]}{_fileHelper.GetExtension(file)}", file));

            await Task.WhenAll(fileTasks);

            DirectoryInfoFile directoryInfoFile = await DirectoryHelper.GetInfoFileFromDirectory(fullPath);
            directoryInfoFile.FileInfo.Add(new FileContentInfo
            {
                FileOwnerUuid = requestingUserUuid,
                FilesOwnedByUser = fileNameCollection
            });

            await DirectoryHelper.UpdateInfoFile(fullPath, directoryInfoFile);
        }

        private async Task SaveVideoAsync(string fullPath, IFormFile video)
        {
            await using var ms = new MemoryStream();
            try
            {
                await video.OpenReadStream().CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();
                await File.WriteAllBytesAsync(fullPath, fileBytes);
            }
            finally
            {
                ms.Close();
            }
        }

        private async Task SaveImageAsync(string fullPath, IFormFile image)
        {
            await using var ms = new MemoryStream();
            try
            {
                await image.OpenReadStream().CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();
                await File.WriteAllBytesAsync(fullPath, fileBytes);
            }
            finally
            {
                ms.Close();
            }
        }
    }
}