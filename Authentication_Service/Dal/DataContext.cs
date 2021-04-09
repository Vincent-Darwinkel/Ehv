using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<UserDto> User { get; set; }
        public virtual DbSet<RefreshTokenDto> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.UserUuid);
            });

            modelBuilder.Entity<RefreshTokenDto>(entity =>
            {
                entity.HasKey(rt => rt.UserUuid);
            });
        }
    }
}
