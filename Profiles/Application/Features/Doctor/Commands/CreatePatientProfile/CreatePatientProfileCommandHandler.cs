using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.CreatePatientProfile
{
    public sealed class CreatePatientProfileCommandHandler : IRequestHandler<CreatePatientProfileCommand, Guid>
    {
        private readonly IPatientRepository _patientRepository;

        public CreatePatientProfileCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Guid> Handle(CreatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            var newProfile = new PatientProfile
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsLinkedToAccount = true
            };

            var newProfileId = await _patientRepository.AddAsync(newProfile, cancellationToken);

            return newProfileId;
        }
    }
}
