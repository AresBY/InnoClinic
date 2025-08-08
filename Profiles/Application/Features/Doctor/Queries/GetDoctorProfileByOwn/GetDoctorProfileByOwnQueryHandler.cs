using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileByOwn;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries;

/// <summary>
/// Handles retrieving detailed information about a doctor by the owner's ID (account ID).
/// </summary>
public sealed class GetDoctorProfileByOwnQueryHandler : IRequestHandler<GetDoctorProfileByOwnQuery, DoctorProfileDto>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetDoctorProfileByOwnQueryHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<DoctorProfileDto> Handle(GetDoctorProfileByOwnQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetDoctorProfileByUserIdAsync(request.OwnerId, cancellationToken);

        if (doctor is null)
        {
            throw new KeyNotFoundException($"Doctor with account ID '{request.OwnerId}' was not found.");
        }

        return doctor.ToDto();
    }
}
