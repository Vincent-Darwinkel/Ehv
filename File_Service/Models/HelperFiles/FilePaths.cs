using System.Collections.Generic;

namespace File_Service.HelperFiles
{
    // Path names must end with an /
    public static class FilePaths
    {
        public const string PublicImageFolder = "/Media/Images/Public/";
        public const string AvatarImageFolder = "/Media/Images/Avatar/";
        public const string PublicVideoFolder = "/Media/Videos/Public/";
        public static readonly List<string> AllowedImagePaths = new List<string>
        {
            PublicImageFolder,
            AvatarImageFolder
        };
        public static readonly List<string> AllowedVideoPaths = new List<string>
        {
            PublicVideoFolder
        };
    }
}