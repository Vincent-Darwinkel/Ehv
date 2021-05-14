using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;
using User_Service.Models;
using UserDto = Authentication_Service.Models.Dto.UserDto;

namespace Authentication_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<UserDto> User { get; set; }
        public virtual DbSet<RefreshTokenDto> RefreshToken { get; set; }
        public virtual DbSet<PendingLoginDto> PendingLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
            });
            modelBuilder.Entity<RefreshTokenDto>(entity =>
            {
                entity.HasKey(rt => rt.UserUuid);
            });
            modelBuilder.Entity<PendingLoginDto>(entity =>
            {
                entity.HasKey(pl => pl.Uuid);
            });
        }
    }
}