using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using User_Service.Dal.Interfaces;
using User_Service.Models;

namespace User_Service.Dal
{
    public class PasswordResetDal : IPasswordResetDal
    {
        private readonly DataContext _context;

        public PasswordResetDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(PasswordResetDto passwordReset)
        {
            await _context.PasswordReset.AddAsync(passwordReset);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetDto> Find(string code)
        {
            return await _context.PasswordReset
                .FirstOrDefaultAsync(pr => pr.Code == code);
        }

        public async Task Delete(Guid uuid)
        {
            PasswordResetDto passwordResetToDelete = await _context.PasswordReset
                .FindAsync(uuid);
            _context.Remove(passwordResetToDelete);
            await _context.SaveChangesAsync();
        }
    }
}