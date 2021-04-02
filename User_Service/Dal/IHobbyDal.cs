using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal
{
    public interface IHobbyDal
    {
        /// <summary>
        /// Adds the hobby to the database
        /// </summary>
        /// <param name="hobby">The hobby object to add</param>
        Task Add(UserHobbyDto hobby);

        /// <returns>All hobbies in the database</returns>
        Task<List<UserHobbyDto>> All();

        /// <summary>
        /// Updates the hobby
        /// </summary>
        /// <param name="hobby">The hobby object to update</param>
        Task Update(UserHobbyDto hobby);

        /// <summary>
        /// Deletes the hobby by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the hobby to remove</param>
        Task Delete(Guid uuid);
    }
}
