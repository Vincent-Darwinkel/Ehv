using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Favorite_Artist_Service.Dal.Interfaces;
using Favorite_Artist_Service.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task Delete(Guid uuid)
        {
            FavoriteArtistDto favoriteArtistToRemove = await _context.FavoriteArtist
                .FindAsync(uuid);
            _context.FavoriteArtist.Remove(favoriteArtistToRemove);
            await _context.SaveChangesAsync();
        }
    }
}