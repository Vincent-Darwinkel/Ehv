using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using Newtonsoft.Json;

namespace File_Service.Models.HelperFiles
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// Checks if the given string starts with an valid file fullPath
        /// </summary>
        /// <param name="path">The fullPath to check</param>
        /// <returns>True if fullPath is valid</returns>
        private static bool PathStartsWithValidDirectory(string path)
        {
            int pathLength = path.Length;
            foreach (var filePath in FilePathInfo.AvailablePaths)
            {
                int filePathLength = filePath.Path.Length;
                for (int i = 0; i < filePathLength; i++)
                {
                    if (i == pathLength)
                    {
                        return false;
                    }
                    if (filePath.Path[i] != path[i])
                    {
                        break;
                    }

                    if (filePath.Path == path.Substring(0, i + 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the supplied user path is not null only contains alphabet and numbers and starts with the allowed paths
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if path is valid, false if not valid</returns>
        public static bool PathIsValid(string path)
        {
            return !string.IsNullOrEmpty(path) &&
                   Regex.IsMatch(path, "[\\/A-Za-z0-9]") &&
                   PathStartsWithValidDirectory(path);
        }

        /// <summary>
        /// Returns the name of all folders in a fullPath
        /// </summary>
        /// <returns>An IEnumerable with the names of all folders in the fullPath</returns>
        public static IEnumerable<string> GetFoldersInDirectory(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException();
            }

            var directoryInfo = new DirectoryInfo(fullPath);
            return directoryInfo.GetDirectories()
                .Select(di => di.Name);
        }

        private static List<string> FilterFiles(List<string> files)
        {
            files.RemoveAll(file => !file.EndsWith(FileExtension.Webp) && !file.EndsWith(FileExtension.Mp4));
            return files;
        }

        /// <summary>
        /// Find all the files in a fullPath
        /// </summary>
        /// <param name="fullPath">The fullPath to search the files in</param>
        /// <returns></returns>
        public static List<string> GetFilesInDirectory(string fullPath)
        {
            var directoryInfo = new DirectoryInfo(fullPath);
            List<string> foundFiles = directoryInfo.GetFiles().Select(file => file.Name)
                .ToList();

            return FilterFiles(foundFiles);
        }

        /// <summary>
        /// Removes the last end of a fullPath
        /// </summary>
        /// <param name="path">The fullPath to fix</param>
        /// <returns>A fullPath with no / at end</returns>
        public static string FixPath(string path)
        {
            return path.EndsWith("/") ? path.Remove(path.Length - 1) : path;
        }

        /// <summary>
        /// Creates a fullPath if the following conditions are met:
        /// <list type="bullet">
        /// <item>
        /// <description>The fullPath does not exists</description>
        /// </item>
        /// <item>
        /// <description>The max allowed sub folders for the fullPath is not reached</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="fullPath">The full fullPath of the folder to create</param>
        /// <param name="userSpecifiedPath">The fullPath the user specified from the front-end</param>
        public static async Task Create(string fullPath, string userSpecifiedPath, Guid userUuid)
        {
            if (Directory.Exists(fullPath))
            {
                throw new DuplicateNameException();
            }

            FilePath filepath = FilePathInfo.Find(userSpecifiedPath);
            if (filepath == null)
            {
                throw new UnprocessableException();
            }

            string rootPath = $"{Environment.CurrentDirectory}/Media{filepath.Path}";
            if (Directory.GetDirectories(rootPath).Length >= filepath.FilePathOptions.MaxAllowedSubFolders)
            {
                throw new UnprocessableException();
            }

            fullPath = FixPath(fullPath);
            Directory.CreateDirectory(fullPath);
            var directoryInfoFile = new DirectoryInfoFile
            {
                DirectoryOwnerUuid = userUuid
            };

            await UpdateInfoFile(fullPath, directoryInfoFile);
        }

        /// <summary>
        /// Gets the info file from the specified directory
        /// </summary>
        /// <param name="fullPath">The full path to search the file</param>
        /// <returns>The found file converted to an DirectoryInfoFile object</returns>
        public static async Task<DirectoryInfoFile> GetInfoFileFromDirectory(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new UnprocessableException();
            }

            string json = await File.ReadAllTextAsync($"{fullPath}/info.json");
            var directoryInfoFile = JsonConvert.DeserializeObject<DirectoryInfoFile>(json);
            directoryInfoFile.FileInfo ??= new List<FileContentInfo>();

            return directoryInfoFile;
        }

        /// <summary>
        /// Updates the info file in the specified directory
        /// </summary>
        /// <param name="fullPath">The full path save the file</param>
        /// <param name="directoryInfoFile">The file to save</param>
        public static async Task UpdateInfoFile(string fullPath, DirectoryInfoFile directoryInfoFile)
        {
            if (string.IsNullOrEmpty(fullPath) || directoryInfoFile == null ||
                directoryInfoFile == new DirectoryInfoFile())
            {
                throw new UnprocessableException();
            }

            string newJson = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(directoryInfoFile));
            await File.WriteAllTextAsync($"{fullPath}/info.json", newJson);
        }

        public static bool CanUploadInDirectory(string userSpecifiedPath)
        {
            FilePath filePathInfo = FilePathInfo.Find(userSpecifiedPath);
            if (filePathInfo.FilePathOptions.AllowFileUploadInRoot && userSpecifiedPath == filePathInfo.Path)
            {
                return true;
            }

            return !filePathInfo.FilePathOptions.AllowFileUploadInRoot && userSpecifiedPath != filePathInfo.Path;
        }
    }
}