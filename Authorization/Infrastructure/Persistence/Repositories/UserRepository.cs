using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        return _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public Task<User?> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return _context.Users
           .AsNoTracking()
           .FirstOrDefaultAsync(p => p.RefreshToken == refreshToken, cancellationToken);
    }
}
