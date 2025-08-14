using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.UpdatePatientProfile
{
    public sealed class UpdatePatientProfileCommandHandler : IRequestHandler<UpdatePatientProfileCommand, Unit>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public UpdatePatientProfileCommandHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Unit> Handle(UpdatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
            if (patient == null)
                throw new KeyNotFoundException("Patient not found");

            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.MiddleName = request.MiddleName;
            patient.PhoneNumber = request.PhoneNumber;
            patient.DateOfBirth = request.DateOfBirth;

            await _patientRepository.UpdateAsync(patient, cancellationToken);

            return Unit.Value;
        }
    }
}
