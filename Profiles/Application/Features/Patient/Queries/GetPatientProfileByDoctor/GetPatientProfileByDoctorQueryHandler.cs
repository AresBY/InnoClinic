using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByDoctor
{
    public class GetPatientProfileByDoctorQueryHandler : IRequestHandler<GetPatientProfileByDoctorQuery, PatientProfileDto>
    {
        private readonly IPatientRepository _repository;

        public GetPatientProfileByDoctorQueryHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientProfileDto> Handle(GetPatientProfileByDoctorQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetPatientProfileByUserIdAsync(request.PatientId, cancellationToken);

            if (profile is null)
                throw new NotFoundException($"Profile with OwnerId {request.PatientId} not found.");

            return profile.ToDto();
        }
    }
}
