using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll
{
    public class GetDoctorProfileAllQueryHandler : IRequestHandler<GetDoctorProfileAllQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorProfileRepository _repository;

        public GetDoctorProfileAllQueryHandler(IDoctorProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DoctorProfileDto>> Handle(GetDoctorProfileAllQuery request, CancellationToken cancellationToken)
        {

            var doctors = await _repository.GetDoctorsAllAsync(cancellationToken);

            var result = doctors.Select(d => d.ToDto()).ToList();

            return result;
        }
    }
}
