using File_Service.Models.HelperFiles.CustomValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace File_Service.Models.FromFrontend
{
    public class FileUpload
    {
        [Required]
        [MustHaveOneElement]
        public List<IFormFile> Files { get; set; }
        [Required]
        [PathIsValid]
        public string Path { get; set; }
    }
}
