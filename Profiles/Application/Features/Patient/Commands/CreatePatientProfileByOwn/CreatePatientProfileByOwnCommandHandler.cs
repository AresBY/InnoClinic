using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile
{
    public sealed class CreatePatientProfileByOwnCommandHandler : IRequestHandler<CreatePatientProfileByOwnCommand, Guid>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public CreatePatientProfileByOwnCommandHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Guid> Handle(CreatePatientProfileByOwnCommand request, CancellationToken cancellationToken)
        {
            var newProfile = request.ToEntity();

            var newProfileId = await _patientRepository.AddAsync(newProfile, cancellationToken);

            return newProfileId;
        }
    }
}
