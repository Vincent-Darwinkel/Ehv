using System;
using System.Collections.Generic;
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