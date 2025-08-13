using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsByOfficeForReceptionist
{
    public sealed class GetDoctorsByOfficeForReceptionistQueryHandler
        : IRequestHandler<GetDoctorsByOfficeForReceptionistQuery, List<DoctorProfileDto>>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public GetDoctorsByOfficeForReceptionistQueryHandler(IDoctorProfileRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorProfileDto>> Handle(
            GetDoctorsByOfficeForReceptionistQuery request,
            CancellationToken cancellationToken)
        {
            var query = _doctorRepository.Query();

            if (request.OfficeId.HasValue)
                query = query.Where(d => d.OfficeId == request.OfficeId.Value);

            return await query
                .Select(d => d.ToDto())
                .ToListAsync(cancellationToken);
        }
    }
}
