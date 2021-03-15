using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.HelperFiles;
using File_Service.Models.FromFrontend;
using Microsoft.AspNetCore.Http;

namespace File_Service.Logic
{
    public class FileLogic
    {
        private readonly VirusScannerLogic _virusScannerLogic;
        private readonly ConcurrentBag<string> _savedFiles = new ConcurrentBag<string>();

        public FileLogic(VirusScannerLogic virusScannerLogic)
        {
            _virusScannerLogic = virusScannerLogic;
        }

        internal async Task<byte[]> GetFileAsync(Guid uuid, FileType type)
        {
            string requestedFilePath = DirectoryHelper.GetFilePathByUuid(uuid, type);
            if (string.IsNullOrEmpty(requestedFilePath))
            {
                throw new FileNotFoundException();
            }

            return await File.ReadAllBytesAsync(requestedFilePath);
        }

        /// <summary>
        /// Saves the file on the file system if the provided path is valid, the file is an webp image or mp4 video and the file does not contain viruses
        /// </summary>
        /// <param name="data">The data from the front-end</param>
        /// <param name="requestingUserUuid">The uuid of the requesting user</param>
        /// <param name="type">The type of files to save</param>
        /// <returns>A list of the name of the files that are saved</returns>
        internal async Task<List<string>> SaveFileAsync(FileUpload data, Guid requestingUserUuid, FileType type)
        {
            if (data.Files == null || data.Files?.Count == 0)
            {
                throw new UnprocessableException();
            }

            string userPath = $"/Media{(type == FileType.Image ? "/Images" : "/Videos")}{data.Path}";
            if (!DirectoryHelper.PathIsValid(userPath) || !FileHelper.FilesValid(data.Files))
            {
                throw new UnprocessableException();
            }

            string userDirectory = $"{Environment.CurrentDirectory}{userPath}{requestingUserUuid}/";
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            var fileTasks = data.Files.Select(file => SaveIFormFileAsync(userDirectory, file));
            await Task.WhenAll(fileTasks);
            return _savedFiles.ToList();
        }

        /// <summary>
        /// Saves the file to the specified path, before saving the file is scanned for viruses.
        /// If it contains a virus the file is not saved and the user directory is removed if it is empty
        /// </summary>
        /// <param name="userDirectory">The directory of the user folder</param>
        /// <param name="file">The file to save and scan for viruses</param>
        private async Task SaveIFormFileAsync(string userDirectory, IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            byte[] fileBytes = ms.ToArray();
            bool fileContainsVirus = await _virusScannerLogic.FileContainsVirus(fileBytes);
            if (!fileContainsVirus)
            {
                string filePath = userDirectory + Guid.NewGuid() + FileHelper.GetFileExtension(file);
                await File.WriteAllBytesAsync(filePath, fileBytes);
                _savedFiles.Add(file.FileName);
                return;
            }

            if (Directory.GetFiles(userDirectory).Length == 0) // Remove user directory if file contains virus and the user directory is empty
            {
                Directory.Delete(userDirectory);
            }
        }
    }
}
