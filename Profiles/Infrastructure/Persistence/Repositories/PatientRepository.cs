using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ProfileDbContext _context;

        public PatientRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(PatientProfile profile, CancellationToken cancellationToken)
        {
            await _context.Patients.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return profile.Id;
        }
        public async Task<List<PatientProfile>> GetAllPatientsAsync(CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<PatientProfile?> GetPatientProfileByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.OwnerId == ownerId, cancellationToken);
        }
    }
}
