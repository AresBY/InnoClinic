using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllPatients;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetAllPatients
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientProfileDto>>
    {
        private readonly IPatientRepository _patientRepository;

        public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<PatientProfileDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAllPatientsAsync(cancellationToken);

            var result = patients.Select(d => d.ToDto()).ToList();

            return result;
        }
    }
}
