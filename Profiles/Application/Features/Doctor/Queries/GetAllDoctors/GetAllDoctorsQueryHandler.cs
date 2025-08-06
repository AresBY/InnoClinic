using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllDoctors
{
    public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorRepository _repository;

        public GetAllDoctorsQueryHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DoctorProfileDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {

            var doctors = await _repository.GetAllDoctorsAsync(cancellationToken);

            var result = doctors.Select(d => d.ToDto()).ToList();

            return result;
        }
    }
}
