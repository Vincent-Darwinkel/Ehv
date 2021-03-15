using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace File_Service.HelperFiles
{
    internal static class FileHelper
    {
        internal static bool FilesValid(List<IFormFile> files)
        {
            return files.TrueForAll(file => file.FileName.EndsWith(FileExtension.Webp) || file.FileName.EndsWith(FileExtension.Mp4));
        }

        internal static string GetFileExtension(IFormFile file)
        {
            return file.FileName.EndsWith(FileExtension.Webp) ? FileExtension.Webp : FileExtension.Mp4;
        }
    }
}
