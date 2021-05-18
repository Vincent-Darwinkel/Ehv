using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User_Service.Enums;
using User_Service.Models;

namespace User_Service.Dal.Interfaces
{
    public interface IUserDal
    {
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="user">The user object to add</param>
        Task Add(UserDto user);

        /// <summary>
        /// Finds the user by uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user to search for</param>
        /// <returns>The found user, null if not found</returns>
        Task<UserDto> Find(Guid userUuid);

        /// <summary>
        /// Finds all users which match the uuid in the collection
        /// </summary>
        /// <param name="uuidCollection">The uuid collection</param>
        /// <returns>The found users, null if nothing is found</returns>
        Task<List<UserDto>> Find(List<Guid> uuidCollection);

        /// <summary>
        /// Counts the users by account role
        /// </summary>
        /// <param name="accountRole">The role to search for</param>
        /// <returns>The total amount of users with the specified role</returns>
        Task<int> Count(AccountRole accountRole);

        /// <summary>
        /// Checks if an account with the username or email exists
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <param name="email">The email to search for</param>
        /// <returns>True if username or email exists, false if not</returns>
        Task<bool> Exists(string username, string email);

        /// <returns>All users in the database</returns>
        Task<List<UserDto>> All();

        Task<bool> Any();

        /// <summary>
        /// Updates the existing user in the database
        /// </summary>
        /// <param name="user">The updated user object</param>
        Task Update(UserDto user);

        /// <summary>
        /// Deletes the user with the matching uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user to remove</param>
        Task Delete(Guid userUuid);
    }
}