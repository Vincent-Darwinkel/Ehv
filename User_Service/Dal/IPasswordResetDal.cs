using System;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal
{
    interface IPasswordResetDal
    {
        /// <summary>
        /// Adds the password reset object in the database
        /// </summary>
        /// <param name="passwordReset">The password reset object to add</param>
        Task Add(PasswordResetDto passwordReset);

        /// <summary>
        /// Finds the password reset object by code
        /// </summary>
        /// <param name="code">The code to search for</param>
        /// <returns>The found password reset object, null if nothing is returned</returns>
        Task<PasswordResetDto> Find(string code);

        /// <summary>
        /// Deletes the password reset object by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the password reset object to remove</param>
        Task Delete(Guid uuid);
    }
}