using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hobby_Service.Dal.Interfaces;
using Hobby_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Hobby_Service.Dal
{
    public class HobbyDal : IHobbyDal
    {
        private readonly DataContext _context;

        public HobbyDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(HobbyDto hobby)
        {
            await _context.Hobby.AddAsync(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HobbyDto>> All()
        {
            return await _context.Hobby.ToListAsync();
        }

        public async Task Update(HobbyDto hobby)
        {
            _context.Hobby.Update(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid uuid)
        {
            HobbyDto hobbyToDelete = await _context.Hobby.FindAsync(uuid);
            _context.Hobby.Remove(hobbyToDelete);
            await _context.SaveChangesAsync();
        }
    }
}