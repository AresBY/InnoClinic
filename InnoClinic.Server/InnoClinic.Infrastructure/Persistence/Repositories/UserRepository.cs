using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Server.Infrastructure.Persistence.Repositories;

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
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Логируй или просто выбрось с информацией
            throw new Exception("Error saving changes to DB", ex);
        }

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
