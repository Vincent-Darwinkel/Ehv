using File_Service.CustomExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            string json = await File.ReadAllTextAsync($"{fullPath}info.json");
            if (string.IsNullOrEmpty(json))
            {
                throw new UnprocessableException();
            }

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

        /// <summary>
        /// Checks if an folder can be created in the parent path. Folders can be created if the directory does not contain files,
        /// the parent directory allows that folders can be created
        /// </summary>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public static bool CanCreateFolderInDirectory(string parentPath)
        {
            if (!PathIsValid(parentPath))
            {
                return false;
            }

            FilePath filePathInfo = FilePathInfo.Find(parentPath);
            if (!filePathInfo.FilePathOptions.AllowFolderCreation)
            {
                return false;
            }

            string fullPath = FixPath($"{Environment.CurrentDirectory}/Media{parentPath}");
            if (GetFilesInDirectory(fullPath).Any())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if files can be uploaded in a directory. Files can be uploaded if the directory allows file upload in root,
        /// if files can be uploaded in path and does not contain sub directories
        /// </summary>
        /// <param name="parentPath">The path the user wants to upload to</param>
        /// <param name="fullPath">The full path</param>
        /// <param name="userUuid">The uuid of the requesting user</param>
        /// <returns>True if the above conditions are met false if not</returns>
        public static async Task<bool> CanUploadFilesInDirectory(string parentPath, string fullPath, Guid userUuid)
        {
            if (!Directory.Exists(fullPath))
            {
                return false;
            }
            if (!PathIsValid(parentPath))
            {
                return false;
            }

            bool directoryContainsFolders = GetFoldersInDirectory(fullPath).Any();
            if (directoryContainsFolders)
            {
                return false;
            }

            FilePath filePathInfo = FilePathInfo.Find(parentPath);
            if (!filePathInfo.FilePathOptions.AllowFileUploadInRoot &&
                parentPath == filePathInfo.Path)
            {
                return false;
            }

            var directoryInfoFile = await GetInfoFileFromDirectory($"{Environment.CurrentDirectory}/Media{filePathInfo.Path}");
            if (!filePathInfo.FilePathOptions.AllowMultipleFilesByUser && directoryInfoFile.FileInfo.Exists(fi => fi.FileOwnerUuid == userUuid))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all directory info files which contains the user uuid
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteFilesOwnedByUser(Guid userUuid)
        {
            string fullPath = $"{Environment.CurrentDirectory}/Media";
            string[] directoryInfoPathCollection = Directory.GetFiles(fullPath, "info.json");

            var directoryInfoFileCollection = new ConcurrentDictionary<string, DirectoryInfoFile>();
            var fileTasks = directoryInfoPathCollection.Select(directoryInfoPath => new Task(async () =>
                {
                    if (!File.Exists(directoryInfoPath)) return;
                    string json = await File.ReadAllTextAsync(directoryInfoPath);
                    var directoryInfoFile = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<DirectoryInfoFile>(json));

                    directoryInfoFileCollection.TryAdd(directoryInfoPath, directoryInfoFile);
                }))
                .ToList();

            await Task.WhenAll(fileTasks);
            await DeleteFilesOwnedByUser(userUuid, directoryInfoFileCollection);
        }

        private static async Task DeleteFilesOwnedByUser(Guid userUuid, ConcurrentDictionary<string, DirectoryInfoFile> directoryInfoFileCollection)
        {
            var allFilesOwnedByUser = new ConcurrentBag<Guid>();
            var updateInfoFileTasks = directoryInfoFileCollection.Select(directoryInfoFileKeyValue => new Task(async () =>
                {
                    string path = directoryInfoFileKeyValue.Key;
                    DirectoryInfoFile directoryInfoFile = directoryInfoFileKeyValue.Value;
                    FileContentInfo fileContentInfo = directoryInfoFile.FileInfo.Find(fi => fi.FileOwnerUuid == userUuid);

                    fileContentInfo.FilesOwnedByUser.ForEach(uuid => allFilesOwnedByUser.Add(uuid));

                    directoryInfoFile.FileInfo.Remove(fileContentInfo);
                    directoryInfoFile.DirectoryContentInfo.RemoveAll(dci => dci.OwnerUuid == userUuid);

                    if (directoryInfoFile.DirectoryOwnerUuid == userUuid)
                    {
                        directoryInfoFile.DirectoryOwnerUuid = Guid.Empty;
                    }

                    await UpdateInfoFile(path, directoryInfoFile);
                }))
                .ToList();

            await Task.WhenAll(updateInfoFileTasks);
        }
    }
}