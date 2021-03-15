using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File_Service.Enums;

namespace File_Service.HelperFiles
{
    internal static class DirectoryHelper
    {
        /// <summary>
        /// Finds the file by uuid and returns the path of the file
        /// </summary>
        /// <param name="uuid">The uuid of the file to search for</param>
        /// <param name="type">The type of file to search for</param>
        /// <returns>A string with the location of the file</returns>
        internal static string GetFilePathByUuid(Guid uuid, FileType type)
        {
            string path = Environment.CurrentDirectory + (type == FileType.Image ? "/Media/Images/" : "/Media/Videos/");
            return Directory
                .GetFiles(path, $"*{(type == FileType.Image ? FileExtension.Webp : FileExtension.Mp4)}",
                    SearchOption.AllDirectories)
                .FirstOrDefault(fileName => fileName.Contains(uuid.ToString()));
        }

        /// <summary>
        /// Checks if the supplied path matches the allowed paths
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if path is valid, false if not valid</returns>
        internal static bool PathIsValid(string path)
        {
            var allowedPaths = new List<string>();
            allowedPaths.AddRange(FilePaths.AllowedImagePaths);
            allowedPaths.AddRange(FilePaths.AllowedVideoPaths);

            return allowedPaths
                .Exists(p => string
                    .Equals(p, path, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}