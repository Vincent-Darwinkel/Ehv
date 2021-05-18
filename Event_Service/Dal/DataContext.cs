using Event_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<EventDto> Event { get; set; }
        public virtual DbSet<EventDateDto> EventDate { get; set; }
        public virtual DbSet<EventStepDto> EventStep { get; set; }
        public virtual DbSet<EventStepUserDto> EventStepUser { get; set; }
        public virtual DbSet<EventDateUserDto> EventDateUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.Property(e => e.Description).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Location).HasMaxLength(125).IsRequired();
                entity.Property(e => e.Title).HasMaxLength(30).IsRequired();

                entity.HasMany(e => e.EventDates)
                    .WithOne()
                    .HasForeignKey(e => e.EventUuid);

                entity.HasMany(e => e.EventSteps)
                    .WithOne()
                    .HasForeignKey(e => e.EventUuid);
            });

            modelBuilder.Entity<EventDateDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.EventDateUsers)
                    .WithOne()
                    .HasForeignKey(e => e.EventDateUuid);
            });

            modelBuilder.Entity<EventStepDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.EventStepUsers)
                    .WithOne()
                    .HasForeignKey(e => e.EventStepUuid);
            });

            modelBuilder.Entity<EventStepUserDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });

            modelBuilder.Entity<EventDateUserDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}