using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Guid> AddAsync(PatientProfile profile, CancellationToken cancellationToken);

        Task<List<PatientProfile>> GetAllPatientsAsync(CancellationToken cancellationToken);
    }
}
