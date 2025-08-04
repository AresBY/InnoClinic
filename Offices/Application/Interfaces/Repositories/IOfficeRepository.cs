using InnoClinic.Offices.Domain.Entities;

namespace InnoClinic.Offices.Application.Interfaces.Repositories
{
    public interface IOfficeRepository
    {
        Task InsertAsync(Office office, CancellationToken cancellationToken);
        Task<List<Office>> GetAllAsync(CancellationToken cancellationToken);

        Task<Office?> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task UpdateAsync(Office office, CancellationToken cancellationToken);

        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
