using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsBySpecialization
{
    public sealed class GetDoctorsBySpecializationQueryHandler
        : IRequestHandler<GetDoctorsBySpecializationQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public GetDoctorsBySpecializationQueryHandler(IDoctorProfileRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorProfileDto>> Handle(GetDoctorsBySpecializationQuery request, CancellationToken cancellationToken)
        {
            var query = _doctorRepository.Query();

            if (request.Specialization.HasValue)
                query = query.Where(d => d.Specialization == request.Specialization.Value);

            var doctors = await query.ToListAsync(cancellationToken);

            return doctors.Select(d => d.ToDto()).ToList();
        }
    }
}
