using InnoClinic.Services.Domain.Entities;
using InnoClinic.Services.Domain.Enums;

namespace InnoClinic.Services.Application.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task AddAsync(Service service, CancellationToken cancellationToken);
        Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Service service, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Service>> GetActiveServicesByCategoryAsync(ServiceCategory category, CancellationToken cancellationToken);
    }
}
