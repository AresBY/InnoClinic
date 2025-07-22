using InnoClinic.Server.Domain.Entities;

namespace InnoClinic.Server.Application.Interfaces.Repositories;

public interface IPatientRepository
{
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Patient patient, CancellationToken cancellationToken);
    Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Patient> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
