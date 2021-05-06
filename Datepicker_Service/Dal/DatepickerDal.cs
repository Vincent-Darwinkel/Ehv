using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Datepicker_Service.Dal
{
    public class DatepickerDal : IDatepickerDal
    {
        private readonly DataContext _context;

        public DatepickerDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(DatepickerDto datepicker)
        {
            await _context.Datepicker.AddAsync(datepicker);
            await _context.SaveChangesAsync();
        }

        public async Task<DatepickerDto> Find(Guid uuid)
        {
            return await _context.Datepicker.FindAsync(uuid);
        }

        public async Task<bool> Exists(string title)
        {
            return await _context.Datepicker
                .AnyAsync(dp => dp.Title == title);
        }

        public async Task Update(DatepickerDto datepicker)
        {
            _context.Datepicker.Update(datepicker);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid uuid)
        {
            DatepickerDto datepickerToRemove = await _context.Datepicker.FindAsync(uuid);
            _context.Datepicker.Remove(datepickerToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
