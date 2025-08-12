using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.ChangeDoctorStatus
{
    public class ChangeDoctorStatusCommandHandler : IRequestHandler<ChangeDoctorStatusCommand, Unit>
    {
        private readonly IDoctorProfileRepository _repository;

        public ChangeDoctorStatusCommandHandler(IDoctorProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ChangeDoctorStatusCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _repository.GetByIdAsync(request.DoctorId, cancellationToken);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");
            }

            doctor.Status = request.Status;

            await _repository.UpdateAsync(doctor, cancellationToken);

            return Unit.Value;
        }
    }
}
