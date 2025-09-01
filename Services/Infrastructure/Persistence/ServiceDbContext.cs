using InnoClinic.Services.Domain.Entities;
using InnoClinic.Specializations.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Services.Infrastructure.Persistence
{
    public class ServicesDbContext : DbContext
    {
        public ServicesDbContext(DbContextOptions<ServicesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; } = null!;

        public DbSet<Specialization> Specializations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Category)
                      .IsRequired();

                entity.Property(e => e.IsActive)
                      .IsRequired();

            });
        }
    }
}
