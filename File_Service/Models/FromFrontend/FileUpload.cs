using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using File_Service.Models.HelperFiles.CustomValidationAttributes;

namespace File_Service.Models.FromFrontend
{
    public class FileUpload
    {
        [Required]
        [MustHaveOneElement]
        public List<IFormFile> Files { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
