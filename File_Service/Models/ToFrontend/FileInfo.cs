using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File_Service.Enums;

namespace File_Service.Models.ToFrontend
{
    public class FileInfo
    {
        public Guid Uuid { get; set; }
        public bool RequestingUserIsOwner { get; set; }
        public bool IsDirectory { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
    }
}
