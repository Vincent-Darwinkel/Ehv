using File_Service.Enums;
using File_Service.Logic;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Service.Models.HelperFiles
{
    public class FileHelper
    {
        private readonly VirusScannerLogic _virusScannerLogic;
        public FileHelper(VirusScannerLogic virusScannerLogic)
        {
            _virusScannerLogic = virusScannerLogic;
        }

        /// <summary>
        /// Scans the file for viruses
        /// </summary>
        /// <param name="file">The file to scan</param>
        /// <returns>True if file contains virus true, false if file does not contain virus</returns>
        public async Task<string> FileContainsVirus(IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);

            try
            {
                byte[] fileBytes = ms.ToArray();
                return await _virusScannerLogic.FileContainsVirus(fileBytes) ? file.Name : null;
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// Filters infected files and invalid file types
        /// </summary>
        /// <param name="files">The files to filter</param>
        /// <returns>Valid files</returns>
        public async Task<List<IFormFile>> FilterFiles(List<IFormFile> files)
        {
            // remove invalid file types
            string[] allowedFileTypes = { ".webp", ".png", ".jpeg", ".jpg", ".webm", ".mp4", ".mov", ".avi" };
            files.RemoveAll(file => allowedFileTypes
                .Any(aft => aft
                    .EndsWith(file.FileName)));

            // remove infected files
            var fileTasks = files.Select(FileContainsVirus);
            string[] infectedFiles = await Task.WhenAll(fileTasks);
            files.RemoveAll(file => infectedFiles.Contains(file.Name));

            return files;
        }

        public FileType GetFileTypeFromFile(IFormFile file)
        {
            string[] allowedImageTypes = { "image/png", "image/jpg", "image/webp", "image/jpeg" };
            return allowedImageTypes.Any(type => file.ContentType.ToLower() == type) ? FileType.Image : FileType.Video;
        }

        public string GetExtension(IFormFile file)
        {
            FileType type = GetFileTypeFromFile(file);
            return type == FileType.Image ? ".webp" : ".mp4";
        }

        /// <summary>
        /// Finds the file by uuid and returns the path of the file
        /// </summary>
        /// <param name="uuid">The uuid of the file to search for</param>
        /// <returns>A string with the location of the file with the file uuid and extension at the end</returns>
        public static string GetFilePathByUuid(Guid uuid)
        {
            string path = Environment.CurrentDirectory + "/Media/";
            return Directory
                .GetFiles(path, "*", SearchOption.AllDirectories)
                .FirstOrDefault(fileName => fileName
                    .Contains(uuid.ToString()));
        }

        /// <summary>
        /// Finds the path of the directory which contains the file
        /// </summary>
        /// <param name="uuid">The uuid of the file to search for</param>
        /// <returns>A string with the path of the directory which contains the file</returns>
        public static string GetDirectoryPathByFileUuid(Guid uuid)
        {
            string path = Environment.CurrentDirectory + "/Media/";
            string filePath = Directory
                .GetFiles(path, "*", SearchOption.AllDirectories)
                .FirstOrDefault(fileName => fileName
                    .Contains(uuid.ToString()));

            var sb = new StringBuilder(filePath);
            sb.Replace(uuid.ToString(), "");
            sb.Replace(FileExtension.Mp4, "");
            sb.Replace(FileExtension.Webp, "");

            return sb.ToString();
        }
    }
}
