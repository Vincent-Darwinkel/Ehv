using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace File_Service.Models.FromFrontend
{
    public class FileUpload
    {
        public List<IFormFile> Files { get; set; }
        [NotNull]
        public string Path { get; set; }
    }
}
