using AutoMapper;
using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Features.Auth.Queries;
using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Application.Features.Auth.Queries;

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public GetAllPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<PatientDto>>(patients);
    }
}
