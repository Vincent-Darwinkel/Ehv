using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File_Service.Dal.Interfaces;
using File_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Service.Dal
{
    public class FileDal : IFileDal
    {
        private readonly DataContext _context;

        public FileDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(List<FileDto> files)
        {
            await _context.File.AddRangeAsync(files);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FileDto>> Find(List<Guid> uuidCollection)
        {
            return await _context.File
                .Where(file => uuidCollection
                    .Contains(file.Uuid))
                .ToListAsync();
        }

        public async Task<FileDto> Find(Guid uuid)
        {
            return await _context.File.FindAsync(uuid);
        }

        public async Task<List<FileDto>> FindInDirectory(Guid directoryUuid)
        {
            return await _context.File
                .Where(file => file.ParentDirectoryUuid == directoryUuid)
                .ToListAsync();
        }

        public async Task Delete(List<FileDto> files)
        {
            _context.File.RemoveRange(files);
            await _context.SaveChangesAsync();
        }
    }
}