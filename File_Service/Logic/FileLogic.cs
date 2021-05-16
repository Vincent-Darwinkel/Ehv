using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            string fullPath = $"{Environment.CurrentDirectory}/Media{userSpecifiedPath}";

            if (files?.Count == 0 ||
                !await DirectoryHelper.CanUploadFilesInDirectory(userSpecifiedPath, fullPath, requestingUserUuid))
            {
                throw new UnprocessableException();
            }

            List<IFormFile> validFiles = await _fileHelper.FilterFiles(files);
            if (validFiles.Count == 0)
            {
                throw new UnprocessableException();
            }

            var fileNameCollection = new List<Guid>();
            validFiles.ForEach(file => fileNameCollection.Add(Guid.NewGuid()));

            var fileTasks = validFiles.Select((file, index) =>
                _fileHelper.GetFileTypeFromFile(file) == FileType.Image
                    ? SaveImageAsync($"{fullPath}/{fileNameCollection[index]}{_fileHelper.GetExtension(file)}", file)

                    : SaveVideoAsync($"{fullPath}/{fileNameCollection[index]}{_fileHelper.GetExtension(file)}", file));

            await Task.WhenAll(fileTasks);
            await UpdateInfoFileAfterFileUpload(requestingUserUuid, fullPath, fileNameCollection);
        }

        private static async Task UpdateInfoFileAfterFileUpload(Guid requestingUserUuid, string fullPath,
            List<Guid> fileNameCollection)
        {
            DirectoryInfoFile directoryInfoFile = await DirectoryHelper.GetInfoFileFromDirectory(fullPath);
            FileContentInfo fileInfo = directoryInfoFile.FileInfo.Find(fi => fi.FileOwnerUuid == requestingUserUuid);
            if (fileInfo != null)
            {
                fileInfo.FilesOwnedByUser.AddRange(fileNameCollection);
            }
            else
            {
                directoryInfoFile.FileInfo.Add(new FileContentInfo
                {
                    FileOwnerUuid = requestingUserUuid,
                    FilesOwnedByUser = fileNameCollection
                });
            }

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

        /// <summary>
        /// Removes a file by uuid if the user is owner and the file exists
        /// </summary>
        /// <param name="fileUuid">The uuid of the file to remove</param>
        /// <param name="requestingUser">The user that made the request</param>
        public async Task Remove(Guid fileUuid, UserHelper requestingUser)
        {
            if (fileUuid == Guid.Empty)
            {
                throw new UnprocessableException();
            }

            string directoryPath = FileHelper.GetDirectoryPathByFileUuid(fileUuid);
            string fullPath = FileHelper.GetFilePathByUuid(fileUuid);

            DirectoryInfoFile infoFile = await DirectoryHelper.GetInfoFileFromDirectory(directoryPath);
            FileContentInfo fileContentInfo = infoFile.FileInfo
                .Find(fi => fi.FileOwnerUuid == requestingUser.Uuid);

            bool fileCanBeRemovedByRequestingUser = fileContentInfo.FilesOwnedByUser
                .Contains(fileUuid) || requestingUser.AccountRole > AccountRole.User;

            if (!File.Exists(fullPath))
            {
                throw new UnprocessableException();
            }

            if (!fileCanBeRemovedByRequestingUser)
            {
                throw new UnauthorizedAccessException();
            }

            File.Delete(fullPath);
            fileContentInfo.FilesOwnedByUser.Remove(fileUuid);
            await DirectoryHelper.UpdateInfoFile(directoryPath, infoFile);
        }
    }
}