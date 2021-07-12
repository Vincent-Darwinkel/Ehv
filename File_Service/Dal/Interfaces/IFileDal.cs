using System;
using File_Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace File_Service.Dal.Interfaces
{
    public interface IFileDal
    {
        /// <summary>
        /// Adds the files to the database
        /// </summary>
        /// <param name="files">The files to add</param
        Task Add(List<FileDto> files);

        /// <summary>
        /// Finds the files which matches the uuid
        /// </summary>
        /// <param name="uuidCollection">The uuids of the files to search from</param>
        /// <returns>The found files, null if non is found</returns>
        Task<List<FileDto>> Find(List<Guid> uuidCollection);

        /// <summary>
        /// Updates an existing file
        /// </summary>
        /// <param name="file">The file to update</param
        Task Update(FileDto file);

        /// <summary>
        /// Deletes the specified files
        /// </summary>
        /// <param name="files">The files to delete</param
        Task Delete(List<FileDto> files);
    }
}
