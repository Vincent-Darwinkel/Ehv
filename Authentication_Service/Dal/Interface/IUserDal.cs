using System;
using System.Threading.Tasks;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal.Interface
{
    public interface IUserDal
    {
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        Task Add(UserDto user);

        /// <summary>
        /// Finds the user by username
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>The found user, null if nothing is found</returns>
        Task<UserDto> Find(string username);

        /// <summary>
        /// Updates the user in the database
        /// </summary>
        /// <param name="user">The updated user object</param>
        Task Update(UserDto user);

        /// <summary>
        /// Deletes the user by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the user to remove</param>
        Task Delete(Guid uuid);
    }
}
