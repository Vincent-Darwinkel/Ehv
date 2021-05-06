using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Service.Dal
{
    public class RefreshTokenDal : IRefreshTokenDal
    {
        private readonly DataContext _context;

        public RefreshTokenDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshTokenDto refreshToken)
        {
            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokenDto> Find(RefreshTokenDto refreshToken)
        {
            return await _context.RefreshToken
                .FirstOrDefaultAsync(rt => rt.UserUuid == refreshToken.UserUuid);
        }

        public async Task Delete(Guid userUuid)
        {
            RefreshTokenDto refreshTokenToDelete = await _context.RefreshToken
                .FirstOrDefaultAsync(rt => rt.UserUuid == userUuid);
            _context.RefreshToken.Remove(refreshTokenToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOutdatedTokens()
        {
            List<RefreshTokenDto> outdatedTokens = await _context.RefreshToken
                .Where(rt => rt.ExpirationDate < DateTime.Now)
                .ToListAsync();

            if (outdatedTokens.Count != 0)
            {
                _context.RefreshToken.RemoveRange(outdatedTokens);
                await _context.SaveChangesAsync();
            }
        }
    }
}
