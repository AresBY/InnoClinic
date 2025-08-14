
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Persistence.Repositories
{
    public class ReceptionistProfileRepository : IReceptionistProfileRepository
    {
        private readonly ProfileDbContext _context;

        public ReceptionistProfileRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public IQueryable<ReceptionistProfile> Query()
        {
            return _context.Receptionists.AsQueryable();
        }
        public async Task<Guid> AddAsync(ReceptionistProfile profile, CancellationToken cancellationToken)
        {
            await _context.Receptionists.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return profile.Id;
        }
        public async Task DeleteAsync(ReceptionistProfile profile, CancellationToken cancellationToken)
        {
            _context.Receptionists.Remove(profile);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ReceptionistProfile?> GetByIdAsync(Guid receptionistId, CancellationToken cancellationToken)
        {
            return await _context.Receptionists
                .FirstOrDefaultAsync(r => r.Id == receptionistId, cancellationToken);
        }

        public async Task UpdateAsync(ReceptionistProfile profile, CancellationToken cancellationToken)
        {
            _context.Receptionists.Update(profile);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
