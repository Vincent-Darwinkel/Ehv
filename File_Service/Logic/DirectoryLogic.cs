using File_Service.Models.HelperFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            string fullPath = $"{Environment.CurrentDirectory}/Media{path}";
            IEnumerable<string> folders = DirectoryHelper.GetFoldersInDirectory(fullPath)
                .ToList();
            IEnumerable<string> files = DirectoryHelper.GetFilesInDirectory(fullPath);

            var items = new List<string>();
            items.AddRange(folders);
            items.AddRange(files);
            return items;
        }

        public async Task Delete(string path, UserHelper requestingUser)
        {
            string fullPath = $"{Environment.CurrentDirectory}/Media{path}";

        }
    }
}