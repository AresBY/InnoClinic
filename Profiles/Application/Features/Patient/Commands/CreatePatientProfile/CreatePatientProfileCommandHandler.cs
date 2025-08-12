using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile
{
    public sealed class CreatePatientProfileCommandHandler : IRequestHandler<CreatePatientProfileCommand, Guid>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public CreatePatientProfileCommandHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Guid> Handle(CreatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            var newProfile = request.ToEntity();

            var newProfileId = await _patientRepository.AddAsync(newProfile, cancellationToken);

            return newProfileId;
        }
    }
}
