using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Event_Service.Dal
{
    public class EventStepDal : IEventStepDal
    {
        private readonly DataContext _context;

        public EventStepDal(DataContext context)
        {
            _context = context;
        }

        public async Task<EventStepDto> Find(Guid uuid)
        {
            return await _context.EventStep
                .Include(es => es.EventStepUsers)
                .FirstOrDefaultAsync(es => es.Uuid == uuid);
        }
    }
}
