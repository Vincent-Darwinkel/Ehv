using System;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal
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

        public async Task<DisabledUserDto> Find(Guid userUuid)
        {
            return await _context.DisabledUser
                .FindAsync(userUuid);
        }

        public async Task Delete(Guid uuid)
        {
            DisabledUserDto disabledUser = await _context.DisabledUser.FindAsync(uuid);
            _context.DisabledUser.Remove(disabledUser);
            await _context.SaveChangesAsync();
        }
    }
}
