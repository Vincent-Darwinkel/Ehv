using Event_Service.Dal.Interfaces;
using Event_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task Add(EventDto eventToAdd)
        {
            await _context.Event.AddAsync(eventToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task<EventDto> Find(Guid uuid)
        {
            return await _context.Event.FindAsync(uuid);
        }

        public async Task<bool> Exists(string title)
        {
            return await _context.Event.AnyAsync(e => e.Title == title);
        }

        public async Task Update(EventDto eventToUpdate)
        {
            _context.Event.Update(eventToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid uuid)
        {
            EventDto eventToRemove = await _context.Event.FindAsync(uuid);
            _context.Event.Remove(eventToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
