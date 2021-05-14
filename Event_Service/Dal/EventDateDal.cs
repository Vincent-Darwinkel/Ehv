using System;
using System.Threading.Tasks;
using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Service.Dal
{
    public class EventDateDal : IEventDateDal
    {
        private readonly DataContext _context;

        public EventDateDal(DataContext context)
        {
            _context = context;
        }

        public async Task<EventDateDto> Find(Guid eventDateUuid)
        {
            return await _context.EventDate
                .FirstOrDefaultAsync(eventDate => eventDate.Uuid == eventDateUuid);
        }

        public async Task Remove(EventDateDto eventDateToRemove)
        {
            _context.EventDate.Remove(eventDateToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
