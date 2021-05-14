using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User_Service.Dal.Interfaces;
using User_Service.Models;

namespace User_Service.Dal
{
    public class DisabledUserDal : IDisabledUserDal
    {
        private readonly DataContext _context;

        public DisabledUserDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(DisabledUserDto disabledUser)
        {
            await _context.DisabledUser.AddAsync(disabledUser);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Guid>> All()
        {
            return await _context.DisabledUser
                .Select(du => du.UserUuid)
                .ToListAsync();
        }

        public async Task<bool> Exists(Guid userUuid)
        {
            return await _context.DisabledUser
                .AnyAsync(u => u.UserUuid == userUuid);
        }

        public async Task Delete(Guid userUuid)
        {
            DisabledUserDto disabledUser = await _context.DisabledUser
                .FirstOrDefaultAsync(du => du.UserUuid == userUuid);
            _context.DisabledUser.Remove(disabledUser);
            await _context.SaveChangesAsync();
        }
    }
}
