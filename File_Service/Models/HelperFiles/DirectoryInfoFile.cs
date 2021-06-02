using System;
using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    public class FileContentInfo
    {
        /// <summary>
        /// The uuid of the owner of the FilesOwnedByUser
        /// </summary>
        public Guid FileOwnerUuid { get; set; }
        public List<Guid> FilesOwnedByUser { get; set; } = new List<Guid>();
    }

    public class DirectoryContentInfo
    {
        /// <summary>
        /// The uuid of the owner of the DirectoriesOwnedByUser
        /// </summary>
        public Guid OwnerUuid { get; set; }
        public List<string> DirectoriesOwnedByUser { get; set; } = new List<string>();

    }

    public class DirectoryInfoFile
    {
        /// <summary>
        /// The owner of the directory
        /// </summary>
        public Guid DirectoryOwnerUuid { get; set; }

        /// <summary>
        /// List of files owned by different users
        /// </summary>
        public List<FileContentInfo> FileInfo { get; set; } = new List<FileContentInfo>();

        /// <summary>
        /// List of directories owned by different users
        /// </summary>
        public List<DirectoryContentInfo> DirectoryContentInfo { get; set; } = new List<DirectoryContentInfo>();
    }
}
