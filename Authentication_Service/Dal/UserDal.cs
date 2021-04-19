using System;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Service.Dal
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

        public async Task<UserDto> Find(string username)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task Update(UserDto user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid uuid)
        {
            UserDto userToRemove = await _context.User
                .FirstOrDefaultAsync(u => u.Uuid == uuid);
            _context.User.Remove(userToRemove);
            await _context.SaveChangesAsync();
        }
    }
}