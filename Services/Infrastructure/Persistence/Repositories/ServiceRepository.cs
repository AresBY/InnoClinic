using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Services.Domain.Enums;
using InnoClinic.Services.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Services.Infrastructure.Repositories
{
    public sealed class ServiceRepository : IServiceRepository
    {
        private readonly ServicesDbContext _context;

        public ServiceRepository(ServicesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Service service, CancellationToken cancellationToken)
        {
            await _context.Services.AddAsync(service, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Services.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Services.ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Service service, CancellationToken cancellationToken)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FindAsync(new object[] { id }, cancellationToken);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IReadOnlyList<Service>> GetActiveServicesByCategoryAsync(ServiceCategory category, CancellationToken cancellationToken)
        {
            return await _context.Services
                .Where(s => s.Status && s.Category == category)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
    }
}
