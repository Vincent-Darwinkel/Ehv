using System;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Service.Dal
{
    public class ActivationDal : IActivationDal
    {
        private readonly DataContext _context;

        public ActivationDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(ActivationDto activation)
        {
            await _context.Activation.AddAsync(activation);
            await _context.SaveChangesAsync();
        }

        public async Task<ActivationDto> Find(string code, Guid userUuid)
        {
            return await _context.Activation
                .FirstOrDefaultAsync(a => a.Code == code &&
                                          a.UserUuid == userUuid);
        }

        public async Task Delete(Guid uuid)
        {
            ActivationDto activation = await _context.Activation
                .FindAsync(uuid);
            _context.Activation.Remove(activation);
            await _context.SaveChangesAsync();
        }
    }
}