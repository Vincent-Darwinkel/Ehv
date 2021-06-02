using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Service.Dal
{
    public class EventDateUserDal : IEventDateUserDal
    {
        private readonly DataContext _context;

        public EventDateUserDal(DataContext context)
        {
            _context = context;
        }

        public async Task<EventDateUserDto> Find(Guid eventDateUuid, Guid userUuid)
        {
            return await _context.EventDateUser
                .FirstOrDefaultAsync(eventDateUser =>
                    eventDateUser.EventDateUuid == eventDateUuid && eventDateUser.UserUuid == userUuid);
        }

        public async Task Remove(EventDateUserDto eventDateUserToRemove)
        {
            _context.EventDateUser.Remove(eventDateUserToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid userUuid)
        {
            List<EventDateUserDto> eventDateUsersToRemove = await _context.EventDateUser
                .Where(edu => edu.UserUuid == userUuid)
                .ToListAsync();

            _context.EventDateUser.RemoveRange(eventDateUsersToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
