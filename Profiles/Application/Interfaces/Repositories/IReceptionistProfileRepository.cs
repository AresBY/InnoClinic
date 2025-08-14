
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories
{
    public interface IReceptionistProfileRepository
    {
        Task<Guid> AddAsync(ReceptionistProfile profile, CancellationToken cancellationToken);
    }
}
