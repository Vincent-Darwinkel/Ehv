using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Models.HelperFiles;

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
        /// Creates an new folder if path is valid
        /// </summary>
        /// <param name="userSpecifiedPath">The path ending with the folder to create</param>
        /// <param name="userUuid">The uuid of the requesting user</param>
        public async Task CreateFolder(string userSpecifiedPath, Guid userUuid)
        {
            if (!DirectoryHelper.PathIsValid(userSpecifiedPath))
            {
                throw new UnprocessableException();
            }
            string fullPath = $"{Environment.CurrentDirectory}/Media{DirectoryHelper.FixPath(userSpecifiedPath)}";
            await DirectoryHelper.Create(fullPath, userSpecifiedPath, userUuid);
        }
    }
}