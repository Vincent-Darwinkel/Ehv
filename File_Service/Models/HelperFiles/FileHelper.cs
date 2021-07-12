using File_Service.Logic;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
