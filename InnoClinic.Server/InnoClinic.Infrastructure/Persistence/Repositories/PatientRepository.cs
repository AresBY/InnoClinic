using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace InnoClinic.Server.Infrastructure.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Patients.AnyAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<Guid> AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return patient.Id;
    }

    public async Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Patients
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public Task UpdateAsync(Patient patient, CancellationToken cancellationToken)
    {
        _context.Patients.Update(patient);
        return _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

}
