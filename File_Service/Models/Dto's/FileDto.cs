using File_Service.Enums;
using System;

namespace File_Service.Models
{
    public class FileDto
    {
        public Guid Uuid { get; set; }
        public Guid OwnerUuid { get; set; }
        public Guid ParentDirectoryUuid { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public FileType FileType { get; set; }
    }
}