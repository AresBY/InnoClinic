using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

public class ProfileDbContext : DbContext
{
    public DbSet<DoctorProfile> Doctors { get; set; } = null!;
    //public DbSet<AdministratorProfile> Administrators { get; set; } = null!;

    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorProfile>().ToTable("Doctors").UseTpcMappingStrategy();
        //modelBuilder.Entity<AdministratorProfile>().ToTable("Administrators").UseTpcMappingStrategy();

        base.OnModelCreating(modelBuilder);
    }
}
