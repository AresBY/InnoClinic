
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Persistence.Repositories
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        private readonly ProfileDbContext _context;

        public IQueryable<PatientProfile> Query()
        {
            return _context.Patients.AsQueryable();
        }

        public PatientProfileRepository(ProfileDbContext context)
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

        public async Task<PatientProfile?> GetPatientProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.OwnerId == ownerId, cancellationToken);
        }

        public async Task DeleteAsync(PatientProfile entity, CancellationToken cancellationToken)
        {
            _context.Patients.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PatientProfile?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken);
        }

        public async Task UpdateAsync(PatientProfile patient, CancellationToken cancellationToken)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
