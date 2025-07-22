using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IPatientRepository _patientRepository;

        public ConfirmEmailCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (patient == null)
                throw new KeyNotFoundException("Patient not found.");

            if (!patient.IsEmailConfirmed)
            {
                patient.IsEmailConfirmed = true;
                await _patientRepository.UpdateAsync(patient, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
