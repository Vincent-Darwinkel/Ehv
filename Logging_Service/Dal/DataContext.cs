using Logging_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Logging_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<LogDto> Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}