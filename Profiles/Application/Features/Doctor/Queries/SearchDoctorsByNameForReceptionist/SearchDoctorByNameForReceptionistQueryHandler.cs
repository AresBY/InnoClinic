using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByNameForReceptionist
{
    public sealed class SearchDoctorByNameForReceptionistQueryHandler
        : IRequestHandler<SearchDoctorByNameForReceptionistQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public SearchDoctorByNameForReceptionistQueryHandler(IDoctorProfileRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorProfileDto>> Handle(SearchDoctorByNameForReceptionistQuery request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim().ToLower();

            var doctors = await _doctorRepository.Query()
                .Where(d =>
                    (d.FirstName ?? "").ToLower().Contains(name) ||
                    (d.LastName ?? "").ToLower().Contains(name) ||
                    (d.MiddleName ?? "").ToLower().Contains(name))
                .ToListAsync(cancellationToken);

            return doctors.Select(d => d.ToDto()).ToList();
        }
    }
}
