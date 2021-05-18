using Logging_Service.Dal.Interfaces;
using Logging_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging_Service.Dal
{
    public class LogDal : ILogDal
    {
        private readonly DataContext _context;

        public LogDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(LogDto log)
        {
            await _context.Log.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LogDto>> Find(List<Guid> uuidCollection)
        {
            return await _context.Log
                .Where(l => uuidCollection
                    .Contains(l.Uuid))
                .ToListAsync();
        }

        public async Task<List<LogDto>> All()
        {
            return await _context.Log.ToListAsync();
        }

        public async Task Delete(List<Guid> uuidCollection)
        {
            List<LogDto> logsToRemove = await _context.Log
                .Where(l => uuidCollection
                    .Contains(l.Uuid))
                .ToListAsync();

            _context.Log.RemoveRange(logsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}