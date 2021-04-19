using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User_Service.Models;

namespace User_Service.Dal.Interfaces
{
    interface IArtistDal
    {
        /// <summary>
        /// Adds the artist to the database
        /// </summary>
        /// <param name="artist">The artist object to add</param>
        Task Add(FavoriteArtistDto artist);

        /// <returns>All artists in the database</returns>
        Task<List<FavoriteArtistDto>> All();

        /// <summary>
        /// Deletes the artist by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the artist to remove</param>
        Task Delete(Guid uuid);
    }
}