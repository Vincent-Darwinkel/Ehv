using Authentication_Service.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Service.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<UserDto> User { get; set; }
        public virtual DbSet<RefreshTokenDto> RefreshToken { get; set; }
        public virtual DbSet<PasswordResetDto> PasswordReset { get; set; }
        public virtual DbSet<DisabledUserDto> DisabledUser { get; set; }
        public virtual DbSet<ActivationDto> Activation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
            });
            modelBuilder.Entity<PasswordResetDto>(entity =>
            {
                entity.HasKey(pr => pr.Uuid);
            });
            modelBuilder.Entity<RefreshTokenDto>(entity =>
            {
                entity.HasKey(rt => rt.UserUuid);
            });
            modelBuilder.Entity<DisabledUserDto>(entity =>
            {
                entity.HasKey(du => du.Uuid);
            });
            modelBuilder.Entity<ActivationDto>(entity =>
            {
                entity.HasKey(a => a.Uuid);
            });
        }
    }
}