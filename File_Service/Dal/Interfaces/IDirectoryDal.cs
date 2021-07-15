using File_Service.Models;
using System.Threading.Tasks;

namespace File_Service.Dal.Interfaces
{
    public interface IDirectoryDal
    {
        /// <summary>
        /// Finds the directory by path
        /// </summary>
        /// <param name="path">The path to search for</param>
        /// <returns>The found directory object, null if nothing is found</returns>
        Task<DirectoryDto> Find(string path);

        /// <summary>
        /// Checks if an directory with this path already exists
        /// </summary>
        /// <param name="path">The path to search for</param>
        /// <returns>True if directory exists, false if non exists</returns>
        Task<bool> Exists(string path);

        /// <summary>
        /// Adds the directory to the database
        /// </summary>
        /// <param name="directory">The directory to add</param
        Task Add(DirectoryDto directory);

        /// <summary>
        /// Updates an existing directory
        /// </summary>
        /// <param name="directory">The directory to update</param
        Task Update(DirectoryDto directory);

        /// <summary>
        /// Deletes the specified directory
        /// </summary>
        /// <param name="directory">The directory to delete</param
        Task Delete(DirectoryDto directory);
    }
}