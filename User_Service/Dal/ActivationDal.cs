using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User_Service.Dal.Interfaces;
using User_Service.Models;

namespace User_Service.Dal
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

        public async Task<ActivationDto> Find(string code)
        {
            return await _context.Activation
                .FirstOrDefaultAsync(a => a.Code == code);
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