using InnoClinic.Authorization.Domain.Entities;

namespace InnoClinic.Authorization.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
