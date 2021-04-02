
namespace File_Service.Models.FromFrontend
{
    public class FolderUpload
    {
        /// <summary>
        /// The parent path which the folder should be created in
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// The name of the folder to create
        /// </summary>
        public string Name { get; set; }
    }
}
