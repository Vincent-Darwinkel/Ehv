using Event_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<EventDto> Event { get; set; }
        public virtual DbSet<EventDateDto> EventDate { get; set; }
        public virtual DbSet<EventDateUserDto> EventDateUser { get; set; }
        public virtual DbSet<EventStepDto> EventStep { get; set; }
        public virtual DbSet<EventStepUserDto> EventStepUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.EventDates)
                    .WithOne();
                entity.HasMany(e => e.EventSteps)
                    .WithOne();
            });
            modelBuilder.Entity<EventDateDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.EventDateUsers)
                    .WithOne();
            });
            modelBuilder.Entity<EventDateUserDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
            modelBuilder.Entity<EventStepDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.EventStepUsers)
                    .WithOne();
            });
            modelBuilder.Entity<EventStepUserDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}