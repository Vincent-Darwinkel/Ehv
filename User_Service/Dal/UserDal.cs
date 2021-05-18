using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Service.Dal.Interfaces;
using User_Service.Enums;
using User_Service.Models;

namespace User_Service.Dal
{
    public class UserDal : IUserDal
    {
        private readonly DataContext _context;

        public UserDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserDto user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> Find(Guid userUuid)
        {
            return await _context.User
                .Include(u => u.FavoriteArtists)
                .Include(u => u.Hobbies)
                .FirstOrDefaultAsync(u => u.Uuid == userUuid);
        }

        public async Task<List<UserDto>> Find(List<Guid> uuidCollection)
        {
            return await _context.User
                .Include(u => u.FavoriteArtists)
                .Include(u => u.Hobbies)
                .Where(user => uuidCollection
                    .Any(uc => user.Uuid == uc))
                .ToListAsync();
        }

        public async Task<int> Count(AccountRole accountRole)
        {
            return await _context.User
                .CountAsync(u => u.AccountRole == accountRole);
        }

        public async Task<bool> Exists(string username, string email)
        {
            return await _context.User
                .AnyAsync(user => user.Username == username ||
                                  user.Email == email);
        }

        public async Task<List<UserDto>> All()
        {
            return await _context.User
                .Include(u => u.FavoriteArtists)
                .Include(u => u.Hobbies)
                .ToListAsync();
        }

        public async Task<bool> Any()
        {
            return await _context.User.AnyAsync();
        }

        public async Task Update(UserDto user)
        {
            _context.Hobby.RemoveRange(user.Hobbies);
            _context.Artist.RemoveRange(user.FavoriteArtists);

            await _context.Hobby.AddRangeAsync(user.Hobbies);
            await _context.Artist.AddRangeAsync(user.FavoriteArtists);

            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid userUuid)
        {
            UserDto userToRemove = await _context.User
                .FindAsync(userUuid);
            _context.User.Remove(userToRemove);
            await _context.SaveChangesAsync();
        }
    }
}