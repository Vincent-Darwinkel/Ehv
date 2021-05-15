using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User_Service.Dal.Interfaces;
using User_Service.Models;

namespace User_Service.Dal
{
    public class ArtistDal : IArtistDal
    {
        private readonly DataContext _context;

        public ArtistDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(FavoriteArtistDto artist)
        {
            await _context.Artist.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FavoriteArtistDto>> All()
        {
            return await _context.Artist.ToListAsync();
        }

        public async Task Delete(Guid uuid)
        {
            FavoriteArtistDto artistToDelete = await _context.Artist
                .FindAsync(uuid);
            _context.Artist.Remove(artistToDelete);
            await _context.SaveChangesAsync();
        }
    }
}