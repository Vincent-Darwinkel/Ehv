using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    public class FilePathOptions
    {
        /// <summary>
        /// True: only one file can be uploaded in the userSpecifiedPath by a user, the existing file will be overwritten
        /// False: multiple files can be uploaded in the userSpecifiedPath by a user
        /// default is true
        /// </summary>
        public bool AllowMultipleFilesByUser { get; set; } = true;

        /// <summary>
        /// Determines how deep in the userSpecifiedPath folders can be created. Example if set to 1 a sub folder in the userSpecifiedPath cannot contain an other sub folder
        /// Default is -1 (no limit) to enable set to 0 or higher
        /// </summary>
        public short MaxAllowedSubFolders { get; set; } = -1;

        /// <summary>
        /// True: files can be uploaded in the root path
        /// False: files must be uploaded in a sub folder
        /// </summary>
        public bool AllowFileUploadInRoot { get; set; } = true;
    }

    public class FilePath
    {
        public string Path { get; set; }
        public FilePathOptions FilePathOptions { get; set; }
    }

    // Path names must end with an /
    // Below are the paths a user can upload to
    public static class FilePathInfo
    {
        /// <summary>
        /// Finds the filePath object which starts with the provided userSpecifiedPath
        /// </summary>
        /// <param name="userSpecifiedPath">The userSpecifiedPath</param>
        /// <returns>FilePathInfo object if found else null</returns>
        public static FilePath Find(string userSpecifiedPath)
        {
            foreach (var filePath in FilePathInfo.AvailablePaths)
            {
                int filePathLength = filePath.Path.Length;
                for (int i = 0; i < filePathLength; i++)
                {
                    if (filePath.Path[i] != userSpecifiedPath[i])
                    {
                        break;
                    }

                    if (filePath.Path == userSpecifiedPath.Substring(0, i + 1))
                    {
                        return filePath;
                    }
                }
            }

            return null;
        }

        public static readonly List<FilePath> AvailablePaths = new List<FilePath>
        {
            new FilePath
            {
                Path = "/public/gallery/",
                FilePathOptions = new FilePathOptions
                {
                    MaxAllowedSubFolders = 2,
                    AllowFileUploadInRoot = false
                }
            },
            new FilePath
            {
                Path = "/public/avatar/",
                FilePathOptions = new FilePathOptions
                {
                    AllowMultipleFilesByUser = false,
                    MaxAllowedSubFolders = 0
                }
            }
        };
    }
}