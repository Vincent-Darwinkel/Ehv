using Favorite_Artist_Service.Model;
using Microsoft.EntityFrameworkCore;

namespace Favorite_Artist_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<FavoriteArtistDto> FavoriteArtist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteArtistDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}