using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal
{
    interface IDisabledUser
    {
        /// <summary>
        /// Adds the disabled user object to the database
        /// </summary>
        /// <param name="disabledUser">The disabled user object to add</param>
        Task Add(DisabledUserDto disabledUser);

        /// <summary>
        /// Finds the disabled user object by the user uuid
        /// </summary>
        /// <param name="userUuid"></param>
        /// <returns>The found disabled user object, null if nothing is found</returns>
        Task<DisabledUserDto> Find(Guid userUuid);

        /// <summary>
        /// Deletes the disabled user object by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the disabled user</param>
        Task Delete(Guid uuid);
    }
}
