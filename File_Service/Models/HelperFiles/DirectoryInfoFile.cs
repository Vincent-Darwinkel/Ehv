using System;
using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    public class FileContentInfo
    {
        public Guid FileOwnerUuid { get; set; }
        public List<Guid> FilesOwnedByUser { get; set; } = new List<Guid>();
    }

    public class DirectoryContentInfo
    {
        public Guid OwnerUuid { get; set; }
        public List<string> DirectoriesOwnedByUser { get; set; } = new List<string>();

    }

    public class DirectoryInfoFile
    {
        public Guid DirectoryOwnerUuid { get; set; }
        public List<FileContentInfo> FileInfo { get; set; } = new List<FileContentInfo>();
        public List<DirectoryContentInfo> DirectoryContentInfo { get; set; } = new List<DirectoryContentInfo>();
    }
}
