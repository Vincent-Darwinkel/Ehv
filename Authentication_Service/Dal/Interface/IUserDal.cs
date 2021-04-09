using System;
using System.Threading.Tasks;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal.Interface
{
    public interface IUserDal
    {
        /// <summary>
        /// Finds the user by username
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>The found user, null if nothing is found</returns>
        Task<UserDto> Find(string username);

        /// <summary>
        /// Deletes the user by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the user to remove</param>
        Task Delete(Guid uuid);
    }
}
