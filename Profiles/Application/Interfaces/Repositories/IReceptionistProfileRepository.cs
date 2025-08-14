
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories
{
    public interface IReceptionistProfileRepository
    {
        IQueryable<ReceptionistProfile> Query();
        Task<Guid> AddAsync(ReceptionistProfile profile, CancellationToken cancellationToken);
        Task DeleteAsync(ReceptionistProfile profile, CancellationToken cancellationToken);
        Task<ReceptionistProfile?> GetByIdAsync(Guid receptionistId, CancellationToken cancellationToken);
        Task UpdateAsync(ReceptionistProfile profile, CancellationToken cancellationToken);
    }
}
