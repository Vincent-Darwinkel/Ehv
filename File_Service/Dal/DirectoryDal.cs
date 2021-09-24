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
                .FirstOrDefaultAsync(d => d.Path == path);
        }

        public async Task<DirectoryDto> Find(Guid uuid)
        {
            return await _context.Directory
                .FirstOrDefaultAsync(d => d.Uuid == uuid);
        }

        public async Task<List<DirectoryDto>> FindAll(string path)
        {
            List<DirectoryDto> directories = await _context.Directory.ToListAsync();
            return directories.FindAll(d =>
            {
                int index = d.Path.LastIndexOf("/", StringComparison.Ordinal);
                string parentDirectory = d.Path.Substring(0, index);
                return path == parentDirectory;
            });
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
            List<DirectoryDto> directoriesToRemove = await _context.Directory
                .Where(d => d.Path
                    .StartsWith(directory.Path))
                .ToListAsync();

            _context.Directory.RemoveRange(directoriesToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
