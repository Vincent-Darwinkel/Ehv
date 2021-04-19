using System;
using System.Threading.Tasks;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal.Interface
{
    public interface IRefreshTokenDal
    {
        /// <summary>
        /// Adds the refresh token to the database
        /// </summary>
        /// <param name="refreshToken">The refresh token to add</param>
        Task Add(RefreshTokenDto refreshToken);

        /// <summary>
        /// Finds the refresh token by user uuid
        /// </summary>
        /// <param name="refreshToken">The refresh token object of the user</param>
        /// <returns>The found refresh token, null if nothing is found</returns>
        Task<RefreshTokenDto> Find(RefreshTokenDto refreshToken);

        /// <summary>
        /// Deletes refresh token by user uuid
        /// </summary>
        /// <param name="refreshToken">The refresh token object to remove</param>
        Task Delete(RefreshTokenDto refreshToken);

        /// <summary>
        /// Deletes expired refresh tokens in the database
        /// </summary>
        Task DeleteOutdatedTokens();
    }
}