using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Models.FromFrontend;
using File_Service.Models.HelperFiles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace File_Service.Logic
{
    public class DirectoryLogic
    {
        /// <summary>
        /// Returns an IEnumerable of the name of all folders in a userSpecifiedPath
        /// </summary>
        /// <param name="path">The userSpecifiedPath to the directory to get the folders from</param>
        /// <returns>An IEnumerable of the name of all folders</returns>
        public List<string> GetItems(string path)
        {
            if (!DirectoryHelper.PathIsValid(path))
            {
                throw new UnprocessableException();
            }

            string fullPath = $"{Environment.CurrentDirectory}/Media{path}";
            IEnumerable<string> folders = DirectoryHelper.GetFoldersInDirectory(fullPath)
                .ToList();
            IEnumerable<string> files = DirectoryHelper.GetFilesInDirectory(fullPath);

            var items = new List<string>();
            items.AddRange(folders);
            items.AddRange(files);
            return items;
        }

        /// <summary>
        /// Gets the directory info file and returns a modified version which only contains
        /// a list of files owned by the user and if the user is owner of the directory
        /// </summary>
        /// <param name="path">The user specified path</param>
        /// <param name="userUuid">The uuid of the requesting user</param>
        /// <returns>an modified version which only contains
        /// a list of files owned by the user and if the user is owner of the directory</returns>
        public async Task<DirectoryInfoFile> GetDirectoryInfo(string path, Guid userUuid)
        {
            if (!DirectoryHelper.PathIsValid(path))
            {
                throw new UnprocessableException();
            }

            string fullPath = $"{Environment.CurrentDirectory}/Media{path}";
            if (!File.Exists($"{fullPath}info.json"))
            {
                throw new FileNotFoundException();
            }

            DirectoryInfoFile info = await DirectoryHelper.GetInfoFileFromDirectory(fullPath);
            info.FileInfo.RemoveAll(fi => fi.FileOwnerUuid != userUuid);
            if (info.DirectoryOwnerUuid != userUuid)
            {
                info.DirectoryOwnerUuid = Guid.Empty;
            }

            return info;
        }

        /// <summary>
        /// Creates a directory if the following conditions are met:
        /// <list type="bullet">
        /// <item>
        /// <description>The full path does not exists</description>
        /// </item>
        /// <item>
        /// <description>The max allowed sub folders for the full path is not reached</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="folder">The form the user send</param>
        /// <param name="requestingUserUuid">The uuid of the requesting user</param>
        public async Task CreateFolder(FolderUpload folder, Guid requestingUserUuid)
        {
            if (!Directory.Exists($"{Environment.CurrentDirectory}/Media{folder.ParentPath}") ||
                !DirectoryHelper.CanCreateFolderInDirectory(folder.ParentPath))
            {
                throw new UnprocessableException();
            }

            string fullPath = $"{Environment.CurrentDirectory}/Media{folder.ParentPath}{folder.Name}";
            if (Directory.Exists(fullPath))
            {
                throw new DuplicateNameException();
            }

            FilePath filepathInfo = FilePathInfo.Find(folder.ParentPath);
            string rootPath = $"{Environment.CurrentDirectory}/Media{filepathInfo?.Path}";

            var rootDirectoryInfoFile = await DirectoryHelper.GetInfoFileFromDirectory(rootPath);
            rootDirectoryInfoFile.DirectoriesOwnedByUser.Add(folder.Name);

            Directory.CreateDirectory(fullPath);
            var directoryInfoFile = new DirectoryInfoFile
            {
                DirectoryOwnerUuid = requestingUserUuid
            };

            await DirectoryHelper.UpdateInfoFile(rootPath, rootDirectoryInfoFile);
            await DirectoryHelper.UpdateInfoFile(fullPath, directoryInfoFile);
        }
    }
}