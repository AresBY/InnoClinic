using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByOwn
{
    public class GetPatientProfileByOwnHandler : IRequestHandler<GetPatientProfileByOwnQuery, PatientProfileDto>
    {
        private readonly IPatientRepository _repository;

        public GetPatientProfileByOwnHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientProfileDto> Handle(GetPatientProfileByOwnQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetPatientProfileByUserIdAsync(request.OwnerId, cancellationToken);

            if (profile is null)
                throw new NotFoundException($"Profile with OwnerId {request.OwnerId} not found.");

            return profile.ToDto();
        }
    }
}
