using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Datepicker_Service.Dal
{
    public class DatepickerDateDal : IDatepickerDateDal
    {
        private readonly DataContext _context;

        public DatepickerDateDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(List<DatepickerDateDto> dates)
        {
            await _context.DatepickerDate.AddRangeAsync(dates);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DatepickerDateDto>> Find(Guid datepickerUuid)
        {
            return await _context.DatepickerDate
                .Where(dpd => dpd.DatePickerUuid == datepickerUuid)
                .ToListAsync();
        }

        public async Task Delete(List<DatepickerDateDto> dates)
        {
            _context.DatepickerDate.RemoveRange(dates);
            await _context.SaveChangesAsync();
        }
    }
}