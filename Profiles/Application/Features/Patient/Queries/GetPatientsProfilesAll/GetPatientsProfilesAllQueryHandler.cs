using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientsProfilesAll
{
    public class GetPatientsProfilesAllQueryHandler : IRequestHandler<GetPatientsProfilesAllQuery, List<PatientProfileDto>>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientsProfilesAllQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<PatientProfileDto>> Handle(GetPatientsProfilesAllQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAllPatientsAsync(cancellationToken);

            var result = patients.Select(d => d.ToDto()).ToList();

            return result;
        }
    }
}
