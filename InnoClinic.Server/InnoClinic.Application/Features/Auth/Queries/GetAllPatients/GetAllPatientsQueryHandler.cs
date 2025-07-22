using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Extensions;
using InnoClinic.Server.Application.Features.Auth.Queries;
using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Application.Features.Auth.Queries;

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        return patients.Select(p => p.ToDto()).ToList();
    }
}
