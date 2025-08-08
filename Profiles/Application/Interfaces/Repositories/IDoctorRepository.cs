

using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories;

public interface IDoctorRepository
{
    Task<Guid> AddAsync(DoctorProfile profile, CancellationToken cancellationToken);

    Task<List<DoctorProfile>> GetDoctorsAllAsync(CancellationToken cancellationToken);
    Task<DoctorProfile?> GetDoctorProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken);
}
