using System;
using System.Threading.Tasks;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Service.Dal
{
    public class EventStepUserDal : IEventStepUserDal
    {
        private readonly DataContext _context;

        public EventStepUserDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(EventStepUserDto eventStepUser)
        {
            await _context.EventStepUser.AddAsync(eventStepUser);
            await _context.SaveChangesAsync();
        }

        public async Task<EventStepUserDto> Find(Guid eventStepUuid, Guid userUuid)
        {
            return await _context.EventStepUser
                .FirstOrDefaultAsync(esu => esu.EventStepUuid == eventStepUuid && esu.UserUuid == userUuid);
        }

        public async Task Remove(EventStepUserDto eventStepUser)
        {
            _context.Remove(eventStepUser);
            await _context.SaveChangesAsync();
        }
    }
}