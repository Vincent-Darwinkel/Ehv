using Hobby_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Hobby_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<HobbyDto> Hobby { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HobbyDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}