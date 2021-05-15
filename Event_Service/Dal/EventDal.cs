using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_Service.Dal
{
    public class EventDal : IEventDal
    {
        private readonly DataContext _context;

        public EventDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(EventDto eventDto)
        {
            await _context.Event.AddAsync(eventDto);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EventDto>> All()
        {
            return await _context.Event
                    .Include(e => e.EventDates)
                    .ThenInclude(e => e.EventDateUsers)
                    .Include(e => e.EventSteps)
                    .ThenInclude(e => e.EventStepUsers)
                    .ToListAsync();
        }

        public async Task<bool> Exists(string eventName)
        {
            return await _context.Event
                .AnyAsync(e => e.Title == eventName);
        }

        public async Task<EventDto> Find(string eventName)
        {
            return await _context.Event
                     .Include(e => e.EventDates)
                     .ThenInclude(e => e.EventDateUsers)
                     .Include(e => e.EventSteps)
                     .ThenInclude(e => e.EventStepUsers)
                     .FirstOrDefaultAsync(e => e.Title == eventName);
        }

        public async Task<EventDto> Find(Guid uuid)
        {
            return await _context.Event
                .Include(e => e.EventDates)
                .ThenInclude(e => e.EventDateUsers)
                .Include(e => e.EventSteps)
                .ThenInclude(e => e.EventStepUsers)
                .FirstOrDefaultAsync(e => e.Uuid == uuid);
        }

        public async Task Update(EventDto eventToUpdate)
        {
            _context.Event.Update(eventToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(EventDto eventToRemove)
        {
            _context.Event.Remove(eventToRemove);
            await _context.SaveChangesAsync();
        }
    }
}