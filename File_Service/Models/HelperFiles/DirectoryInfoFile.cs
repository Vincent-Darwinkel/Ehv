using System;
using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    public class FileContentInfo
    {
        public Guid FileOwnerUuid { get; set; }
        public List<string> FilesOwnedByUser { get; set; } = new List<string>();
    }

    public class DirectoryInfoFile
    {
        public Guid DirectoryOwnerUuid { get; set; }
        public List<FileContentInfo> FileInfo { get; set; }

    }
}
