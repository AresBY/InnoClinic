
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

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
    }
}
