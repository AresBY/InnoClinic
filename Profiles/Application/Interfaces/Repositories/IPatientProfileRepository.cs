using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories
{
    public interface IPatientProfileRepository
    {
        Task<Guid> AddAsync(PatientProfile profile, CancellationToken cancellationToken);

        Task<List<PatientProfile>> GetAllPatientsAsync(CancellationToken cancellationToken);

        Task<PatientProfile?> GetPatientProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken);
    }
}
