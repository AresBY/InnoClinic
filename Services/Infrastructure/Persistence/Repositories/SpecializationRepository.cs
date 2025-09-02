using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Infrastructure.Persistence;
using InnoClinic.Specializations.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Services.Infrastructure.Repositories
{
    public sealed class SpecializationRepository : ISpecializationRepository
    {
        private readonly ServicesDbContext _dbContext;

        public SpecializationRepository(ServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Specialization specialization, CancellationToken cancellationToken)
        {
            await _dbContext.Specializations.AddAsync(specialization, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Specialization?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Specializations
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Specialization>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Specializations
                .Include(s => s.Services)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Specialization specialization, CancellationToken cancellationToken)
        {
            _dbContext.Specializations.Update(specialization);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var specialization = await _dbContext.Specializations.FindAsync(new object[] { id }, cancellationToken);
            if (specialization != null)
            {
                _dbContext.Specializations.Remove(specialization);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<Specialization?> GetByIdWithServicesAsync(Guid specializationId, CancellationToken cancellationToken)
        {
            return await _dbContext.Specializations
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == specializationId, cancellationToken);
        }
    }
}
