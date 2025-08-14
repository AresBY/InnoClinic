using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByReceptionist
{
    public sealed class GetPatientProfileByReceptionistQueryHandler : IRequestHandler<GetPatientProfileByReceptionistQuery, PatientProfileDto?>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public GetPatientProfileByReceptionistQueryHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientProfileDto?> Handle(GetPatientProfileByReceptionistQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with Id '{request.PatientId}' not found.");

            return new PatientProfileDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth
            };
        }
    }
}
