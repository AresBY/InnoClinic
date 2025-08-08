using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Offices.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ProfileDbContext _context;

        public DoctorRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(DoctorProfile profile, CancellationToken cancellationToken)
        {
            await _context.Doctors.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return profile.Id;
        }

        public async Task<List<DoctorProfile>> GetDoctorsAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<DoctorProfile?> GetDoctorProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.OwnerId == ownerId, cancellationToken);
        }
    }
}
