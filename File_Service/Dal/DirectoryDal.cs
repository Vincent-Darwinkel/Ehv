using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File_Service.Dal.Interfaces;
using File_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Service.Dal
{
    public class DirectoryDal : IDirectoryDal
    {
        private readonly DataContext _context;

        public DirectoryDal(DataContext context)
        {
            _context = context;
        }

        public async Task<DirectoryDto> Find(string path)
        {
            return await _context.Directory
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(string path)
        {
            return await _context.Directory
                .AnyAsync(dir => dir.Path == path);
        }

        public async Task Add(DirectoryDto directory)
        {
            await _context.Directory.AddAsync(directory);
            await _context.SaveChangesAsync();
        }

        public async Task Update(DirectoryDto directory)
        {
            _context.Directory.Update(directory);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(DirectoryDto directory)
        {
            _context.Directory.Remove(directory);
            await _context.SaveChangesAsync();
        }
    }
}
