using Favorite_Artist_Service.Dal.Interfaces;
using Favorite_Artist_Service.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Favorite_Artist_Service.Dal
{
    public class FavoriteArtistDal : IFavoriteArtistDal
    {
        private readonly DataContext _context;

        public FavoriteArtistDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(FavoriteArtistDto favoriteArtist)
        {
            await _context.AddAsync(favoriteArtist);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FavoriteArtistDto>> All()
        {
            return await _context.FavoriteArtist.ToListAsync();
        }

        public async Task Update(FavoriteArtistDto favoriteArtist)
        {
            _context.FavoriteArtist.Update(favoriteArtist);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<Guid> uuidCollection)
        {
            List<FavoriteArtistDto> favoriteArtistsToRemove = await _context.FavoriteArtist
                .Where(a => uuidCollection.Contains(a.Uuid))
                .ToListAsync();

            _context.FavoriteArtist.RemoveRange(favoriteArtistsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}