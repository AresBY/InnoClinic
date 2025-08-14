using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfileByReceptionist
{
    public sealed class CreatePatientProfileByReceptionistCommandHandler
    {
        private readonly IPatientProfileRepository _patientRepository;

        public CreatePatientProfileByReceptionistCommandHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Guid> Handle(CreatePatientProfileByReceptionistCommand request, CancellationToken cancellationToken)
        {
            var patient = request.ToEntity();
            await _patientRepository.AddAsync(patient, cancellationToken);
            return patient.Id;
        }
    }
}
