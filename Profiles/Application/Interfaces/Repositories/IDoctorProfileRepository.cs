

using InnoClinic.Offices.Domain.Enums;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories;

public interface IDoctorProfileRepository
{
    Task<Guid> AddAsync(DoctorProfile profile, CancellationToken cancellationToken);
    Task<List<DoctorProfile>> GetDoctorsAllAsync(DoctorSpecialization? specialization, CancellationToken cancellationToken);
    Task<DoctorProfile?> GetDoctorProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(DoctorProfile existingProfile, CancellationToken cancellationToken);
}
