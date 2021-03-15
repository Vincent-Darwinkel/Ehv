using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace File_Service.Models.FromFrontend
{
    public class FileUpload
    {
        public List<IFormFile> Files { get; set; }
        [NotNull]
        public string Path { get; set; }
    }
}
