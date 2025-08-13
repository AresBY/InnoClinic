using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByName
{
    public sealed class SearchDoctorByNameQueryHandler
        : IRequestHandler<SearchDoctorByNameQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public SearchDoctorByNameQueryHandler(IDoctorProfileRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorProfileDto>> Handle(SearchDoctorByNameQuery request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim().ToLower();

            var doctors = await _doctorRepository.Query()
                .Where(d =>
                      (d.FirstName != null && d.FirstName.ToLower().Contains(name)) ||
                      (d.LastName != null && d.LastName.ToLower().Contains(name)) ||
                      (d.MiddleName != null && d.MiddleName.ToLower().Contains(name)))
                .ToListAsync(cancellationToken);

            return doctors.Select(d => d.ToDto()).ToList();
        }
    }
}
