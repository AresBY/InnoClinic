using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetPatientProfile
{
    public class GetPatientProfileQueryHandler : IRequestHandler<GetPatientProfileQuery, PatientProfileDto>
    {
        private readonly IPatientRepository _repository;

        public GetPatientProfileQueryHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientProfileDto> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetPatientProfileByOwnerIdAsync(request.OwnerId, cancellationToken);

            if (profile is null)
                throw new NotFoundException($"Profile with OwnerId {request.OwnerId} not found.");

            return profile.ToDto();
        }
    }
}
