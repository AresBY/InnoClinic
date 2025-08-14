using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

public class ProfileDbContext : DbContext
{
    public DbSet<DoctorProfile> Doctors { get; set; } = null!;
    public DbSet<PatientProfile> Patients { get; set; } = null!;
    public DbSet<ReceptionistProfile> Receptionists { get; set; } = null!;
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorProfile>().ToTable("Doctors").UseTpcMappingStrategy();
        modelBuilder.Entity<PatientProfile>().ToTable("Patients").UseTpcMappingStrategy();
        modelBuilder.Entity<ReceptionistProfile>().ToTable("Receptionists").UseTpcMappingStrategy();

        base.OnModelCreating(modelBuilder);
    }
}
