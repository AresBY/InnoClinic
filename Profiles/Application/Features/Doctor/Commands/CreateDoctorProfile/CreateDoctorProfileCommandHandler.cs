using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Handlers
{
    public class CreateDoctorProfileCommandHandler : IRequestHandler<CreateDoctorProfileCommand, Guid>
    {
        private readonly IDoctorRepository _profileRepository;

        public CreateDoctorProfileCommandHandler(IDoctorRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Guid> Handle(CreateDoctorProfileCommand request, CancellationToken cancellationToken)
        {
            var doctor = request.ToDoctorProfileEntity();

            return await _profileRepository.AddAsync(doctor, cancellationToken);
        }
    }
}
