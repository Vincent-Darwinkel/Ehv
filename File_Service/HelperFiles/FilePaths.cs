using System.Collections.Generic;

namespace File_Service.HelperFiles
{
    // Path names must end with an /
    internal static class FilePaths
    {
        internal const string PublicImageFolder = "/Media/Images/Public/";
        internal const string AvatarImageFolder = "/Media/Images/Avatar/";
        internal const string PublicVideoFolder = "/Media/Videos/Public/";
        internal static readonly List<string> AllowedImagePaths = new List<string>
        {
            PublicImageFolder,
            AvatarImageFolder
        };
        internal static readonly List<string> AllowedVideoPaths = new List<string>
        {
            PublicVideoFolder
        };
    }
}