using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;

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
                .FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken.RefreshToken &&
                                           rt.UserUuid == refreshToken.UserUuid);
        }

        public async Task Delete(RefreshTokenDto refreshToken)
        {
            _context.RefreshToken.Remove(refreshToken);
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
