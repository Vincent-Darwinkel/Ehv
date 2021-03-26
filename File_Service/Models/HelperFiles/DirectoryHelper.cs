using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File_Service.Enums;
using File_Service.HelperFiles;

namespace File_Service.Models.HelperFiles
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// Finds the file by uuid and returns the path of the file
        /// </summary>
        /// <param name="uuid">The uuid of the file to search for</param>
        /// <returns>A string with the location of the file</returns>
        public static string GetFilePathByUuid(Guid uuid)
        {
            string path = Environment.CurrentDirectory + "/Media/";
            return Directory
                .GetFiles(path, "*")
                .FirstOrDefault(fileName => fileName
                    .Contains(uuid.ToString()));
        }

        /// <summary>
        /// Checks if the supplied path matches the allowed paths
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if path is valid, false if not valid</returns>
        public static bool PathIsValid(string path)
        {
            return FilePaths.AllowedPaths.Exists(path.Contains);
        }
    }
}