using Microsoft.EntityFrameworkCore;
using User_Service.Models;

namespace User_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<UserDto> User { get; set; }
        public virtual DbSet<UserHobbyDto> Hobby { get; set; }
        public virtual DbSet<FavoriteArtistDto> Artist { get; set; }
        public virtual DbSet<PasswordResetDto> PasswordReset { get; set; }
        public virtual DbSet<DisabledUserDto> DisabledUser { get; set; }
        public virtual DbSet<ActivationDto> Activation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
                entity.HasMany(user => user.Hobbies)
                    .WithOne()
                    .HasForeignKey(e => e.UserUuid);

                entity.HasMany(user => user.FavoriteArtists)
                    .WithOne()
                    .HasForeignKey(e => e.UserUuid);
            });
            modelBuilder.Entity<UserHobbyDto>(entity =>
            {
                entity.HasKey(uh => uh.Uuid);
            });
            modelBuilder.Entity<FavoriteArtistDto>(entity =>
            {
                entity.HasKey(fa => fa.Uuid);
            });
            modelBuilder.Entity<DisabledUserDto>(entity =>
            {
                entity.HasKey(du => du.Uuid);
            });
            modelBuilder.Entity<ActivationDto>(entity =>
            {
                entity.HasKey(a => a.Uuid);
            });
            modelBuilder.Entity<PasswordResetDto>(entity =>
            {
                entity.HasKey(pr => pr.Uuid);
            });
        }
    }
}