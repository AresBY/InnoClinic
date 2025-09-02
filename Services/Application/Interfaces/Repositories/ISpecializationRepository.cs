using InnoClinic.Specializations.Domain.Entities;

namespace InnoClinic.Services.Application.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        Task AddAsync(Specialization specialization, CancellationToken cancellationToken);
        Task<Specialization?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Specialization>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Specialization specialization, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Specialization?> GetByIdWithServicesAsync(Guid specializationId, CancellationToken cancellationToken);
    }
}
