using Hobby_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hobby_Service.Dal.Interfaces
{
    public interface IHobbyDal
    {
        /// <summary>
        /// Adds the hobby to the database
        /// </summary>
        /// <param name="hobby">The hobby to add</param>
        Task Add(HobbyDto hobby);

        /// <summary>
        /// Finds all hobbies the database
        /// </summary>
        /// <returns>All hobbies in the database</returns>
        Task<List<HobbyDto>> All();

        /// <summary>
        /// Updates the hobby in the database
        /// </summary>
        /// <param name="hobby">The updated hobby</param>
        Task Update(HobbyDto hobby);

        /// <summary>
        /// Deletes the hobby that matches the uuid
        /// </summary>
        /// <param name="uuid">The uuid of the hobby to delete</param>
        Task Delete(Guid uuid);
    }
}