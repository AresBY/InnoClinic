using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll
{
    public class GetDoctorsAllQueryHandler : IRequestHandler<GetDoctorsAllQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorRepository _repository;

        public GetDoctorsAllQueryHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DoctorProfileDto>> Handle(GetDoctorsAllQuery request, CancellationToken cancellationToken)
        {

            var doctors = await _repository.GetDoctorsAllAsync(cancellationToken);

            var result = doctors.Select(d => d.ToDto()).ToList();

            return result;
        }
    }
}
