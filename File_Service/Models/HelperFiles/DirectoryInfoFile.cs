using System;
using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    public class FileContentInfo
    {
        public Guid FileOwnerUuid { get; set; }
        public List<Guid> FilesOwnedByUser { get; set; } = new List<Guid>();
    }

    public class DirectoryInfoFile
    {
        public Guid DirectoryOwnerUuid { get; set; }
        public List<FileContentInfo> FileInfo { get; set; } = new List<FileContentInfo>();
        public List<string> DirectoriesOwnedByUser { get; set; } = new List<string>();

    }
}
