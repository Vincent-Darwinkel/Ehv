using System;

namespace File_Service.Models
{
    public class DirectoryDto
    {
        public Guid Uuid { get; set; }
        public Guid OwnerUuid { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Not the full path of the directory, but the path the front end can store files in
        /// </summary>
        public string Path { get; set; }
    }
}