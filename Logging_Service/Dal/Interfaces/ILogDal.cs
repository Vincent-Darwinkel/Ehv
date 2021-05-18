using Logging_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging_Service.Dal.Interfaces
{
    public interface ILogDal
    {
        /// <summary>
        /// Adds the log object to the database
        /// </summary>
        /// <param name="log">The log to add</param>
        Task Add(LogDto log);

        /// <returns>All logs in the database</returns>
        Task<List<LogDto>> All();

        /// <summary>
        /// Deletes the logs which matches the uuidCollection in the collection
        /// </summary>
        /// <param name="uuidCollection">The collection of uuidCollection</param>
        Task Delete(List<Guid> uuidCollection);
    }
}