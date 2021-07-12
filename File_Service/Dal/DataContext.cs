using File_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<FileDto> File { get; set; }
        public virtual DbSet<DirectoryDto> Directory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileDto>(entity =>
            {
                entity.HasKey(file => file.Uuid);
            });
            modelBuilder.Entity<DirectoryDto>(entity =>
            {
                entity.HasKey(directory => directory.Uuid);
            });
        }
    }
}