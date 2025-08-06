using InnoClinic.Authorization.Domain.Entities;

using InnoClinicCommon.Enums;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Table per Hierarchy
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired();

                entity.HasDiscriminator<UserRole>("Role")
                      .HasValue<Patient>(UserRole.Patient)
                      .HasValue<Doctor>(UserRole.Doctor)
                      .HasValue<Admin>(UserRole.Admin)
                      .HasValue<Receptionist>(UserRole.Receptionist);
            });
        }
    }
}
