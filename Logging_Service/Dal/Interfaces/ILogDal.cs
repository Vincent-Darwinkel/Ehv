using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging_Service.Models;

namespace Logging_Service.Dal.Interfaces
{
    public interface ILogDal
    {
        /// <summary>
        /// Adds the log object to the database
        /// </summary>
        /// <param name="log">The log to add</param>
        Task Add(LogDto log);

        /// <summary>
        /// Finds the logs that matches the uuidCollection in the collection
        /// </summary>
        /// <param name="uuidCollection">The uuidCollection to find</param>
        /// <returns>The found logs</returns>
        Task<List<LogDto>> Find(List<Guid> uuidCollection);

        /// <summary>
        /// Finds the logs that matches the specified microService name
        /// </summary>
        /// <param name="microService">The name of the micro services to find the logs</param>
        /// <returns>The found logs</returns>
        Task<List<LogDto>> Find(string microService);

        /// <returns>All logs in the database</returns>
        Task<List<LogDto>> All();

        /// <summary>
        /// Deletes the logs which matches the uuidCollection in the collection
        /// </summary>
        /// <param name="uuidCollection">The collection of uuidCollection</param>
        Task Delete(List<Guid> uuidCollection);
    }
}