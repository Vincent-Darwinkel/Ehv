using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User_Service.Dal.Interfaces;
using User_Service.Models;

namespace User_Service.Dal
{
    public class HobbyDal : IHobbyDal
    {
        private readonly DataContext _context;

        public HobbyDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserHobbyDto hobby)
        {
            await _context.Hobby.AddAsync(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserHobbyDto>> All()
        {
            return await _context.Hobby
                .ToListAsync();
        }

        public async Task Update(UserHobbyDto hobby)
        {
            _context.Hobby.Update(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid uuid)
        {
            UserHobbyDto hobbyToDelete = await _context.Hobby
                .FindAsync(uuid);
            _context.Hobby.Remove(hobbyToDelete);
            await _context.SaveChangesAsync();
        }
    }
}