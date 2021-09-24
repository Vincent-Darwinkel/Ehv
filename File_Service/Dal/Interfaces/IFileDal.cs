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
        /// Finds the file which matches the uuid
        /// </summary>
        /// <param name="uuid">The uuid of the file to search from</param>
        /// <returns>The found file, null if non is found</returns>
        Task<FileDto> Find(Guid uuid);

        /// <summary>
        /// Finds the files which matches the specified directory uuid
        /// </summary>
        /// <param name="directoryUuid">The uuid directory</param>
        /// <returns>The found files, null if non is found</returns>
        Task<List<FileDto>> FindInDirectory(Guid directoryUuid);

        /// <summary>
        /// Deletes the specified file
        /// </summary>
        /// <param name="file">The file to delete</param>
        Task Delete(FileDto file);

        /// <summary>
        /// Deletes all files which matches the parent uuid
        /// </summary>
        /// <param name="parentDirectoryUuid">The uuid of the directory</param>
        Task Delete(Guid parentDirectoryUuid);
    }
}
