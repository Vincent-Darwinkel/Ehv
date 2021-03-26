using System.Collections.Generic;

namespace File_Service.Models.HelperFiles
{
    // Path names must end with an /
    // Below are the paths a user can upload to
    public static class FilePaths
    {
        public const string Gallery = "/public/gallery/";
        public static readonly List<string> AllowedPaths = new List<string>
        {
            Gallery
        };
    }
}