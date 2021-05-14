using System.Threading.Tasks;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal.Interface
{
    public interface IPendingLoginDal
    {
        /// <summary>
        /// Adds the pending login
        /// </summary>
        /// <param name="pendingLogin">The pending login to add</param>
        Task AddAsync(PendingLoginDto pendingLogin);

        /// <summary>
        /// Finds an pending login by code or uuid
        /// </summary>
        /// <param name="pendingLogin">The pending login to find</param>
        /// <returns>The found pending login</returns>
        Task<PendingLoginDto> Find(PendingLoginDto pendingLogin);

        /// <summary>
        /// Removes the specified pending login
        /// </summary>
        /// <param name="pendingLogin">The pending login to remove</param>
        Task Remove(PendingLoginDto pendingLogin);

        /// <summary>
        /// Removes expired pending logins
        /// </summary>
        Task RemoveOutdated();
    }
}