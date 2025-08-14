using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.DeletePatientProfile
{
    public class DeletePatientProfileCommandHandler : IRequestHandler<DeletePatientProfileCommand, Unit>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public DeletePatientProfileCommandHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Unit> Handle(DeletePatientProfileCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
            if (patient == null)
                throw new KeyNotFoundException("Patient not found");

            await _patientRepository.DeleteAsync(patient, cancellationToken);
            return Unit.Value;
        }
    }
}
