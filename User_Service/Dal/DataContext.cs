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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
            });
            modelBuilder.Entity<UserHobbyDto>(entity =>
            {
                entity.HasKey(uh => uh.Uuid);
            });
            modelBuilder.Entity<FavoriteArtistDto>(entity =>
            {
                entity.HasKey(fa => fa.Uuid);
            });
        }
    }
}