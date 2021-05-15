using Favorite_Artist_Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Favorite_Artist_Service.Dal.Interfaces
{
    public interface IFavoriteArtistDal
    {
        /// <summary>
        /// Adds the favoriteArtist to the database
        /// </summary>
        /// <param name="favoriteArtist">The favoriteArtist to add</param>
        Task Add(FavoriteArtistDto favoriteArtist);

        /// <summary>
        /// Finds all hobbies the database
        /// </summary>
        /// <returns>All hobbies in the database</returns>
        Task<List<FavoriteArtistDto>> All();

        /// <summary>
        /// Updates the favoriteArtist in the database
        /// </summary>
        /// <param name="favoriteArtist">The updated favoriteArtist</param>
        Task Update(FavoriteArtistDto favoriteArtist);

        /// <summary>
        /// Deletes the favoriteArtist that matches the uuid
        /// </summary>
        /// <param name="uuid">The uuid of the favoriteArtist to delete</param>
        Task Delete(Guid uuid);
    }
}