using System;
using System.Collections.Generic;
using System.Linq;

namespace File_Service.Models.HelperFiles
{
    public static class ValidFilePaths
    {
        private static readonly List<string> ValidPaths = new List<string>
        {
            "/Media/Public/Gallery/"
        };

        /// <summary>
        /// Checks if files can be uploaded in the directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FilePathIsValid(string path)
        {
            return ValidPaths
                .Any(vfp => path.StartsWith(vfp, StringComparison.Ordinal)) &&
                !path.Contains(".") &&
                path.EndsWith("/");
        }
    }
}