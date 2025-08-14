using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories
{
    public interface IPatientProfileRepository
    {
        IQueryable<PatientProfile> Query();
        Task<Guid> AddAsync(PatientProfile profile, CancellationToken cancellationToken);
        Task DeleteAsync(PatientProfile profile, CancellationToken cancellationToken);
        Task<List<PatientProfile>> GetAllPatientsAsync(CancellationToken cancellationToken);
        Task<PatientProfile?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<PatientProfile?> GetPatientProfileByUserIdAsync(Guid ownerId, CancellationToken cancellationToken);
        Task UpdateAsync(PatientProfile patient, CancellationToken cancellationToken);
    }
}
