using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datepicker_Service.Dal
{
    public class DatepickerDatepickerAvailabilityDal : IDatepickerAvailabilityDal
    {
        private readonly DataContext _context;

        public DatepickerDatepickerAvailabilityDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(List<DatepickerAvailabilityDto> availabilities)
        {
            await _context.DatepickerAvailability.AddRangeAsync(availabilities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DatepickerAvailabilityDto>> Find(List<Guid> dateUuidCollection, Guid userUuid)
        {
            return await _context.DatepickerAvailability.Where(dpa =>
                dateUuidCollection.Contains(dpa.DateUuid) && dpa.UserUuid == userUuid)
                .ToListAsync();
        }

        public async Task<List<DatepickerAvailabilityDto>> Find(List<Guid> uuidCollection)
        {
            return await _context.DatepickerAvailability
                .Where(dpa => uuidCollection.Contains(dpa.Uuid))
                .ToListAsync();
        }

        public async Task<DatepickerAvailabilityDto> Find(DatepickerAvailabilityDto availability)
        {
            return await _context.DatepickerAvailability.FirstOrDefaultAsync(a => a.Uuid == availability.Uuid);
        }


        public async Task Delete(IEnumerable<Guid> dateUuidCollection, Guid userUuid)
        {
            IEnumerable<DatepickerAvailabilityDto> availabilitiesToRemove = _context.DatepickerAvailability
                .Where(a => a.UserUuid == userUuid && dateUuidCollection
                    .Contains(a.DateUuid));

            _context.DatepickerAvailability.RemoveRange(availabilitiesToRemove);
            await _context.SaveChangesAsync();
        }
    }
}