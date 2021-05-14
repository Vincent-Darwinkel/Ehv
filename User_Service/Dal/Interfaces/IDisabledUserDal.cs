using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal.Interfaces
{
    public interface IDisabledUserDal
    {
        /// <summary>
        /// Adds the disabled user object to the database
        /// </summary>
        /// <param name="disabledUser">The disabled user object to add</param>
        Task Add(DisabledUserDto disabledUser);

        /// <returns>All disabled users in the database</returns>
        Task<List<Guid>> All();

        /// <summary>
        /// Checks if the user is disabled
        /// </summary>
        /// <param name="userUuid">The user uuid to search for</param>
        /// <returns>True if found, false if not found</returns>
        Task<bool> Exists(Guid userUuid);

        /// <summary>
        /// Deletes the disabled user object by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the disabled user</param>
        Task Delete(Guid uuid);
    }
}
