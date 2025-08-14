using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsForReceptionist
{
    public class GetDoctorsForReceptionistQueryHandler
        : IRequestHandler<GetDoctorsForReceptionistQuery, List<DoctorListItemDto>>
    {
        private readonly IDoctorProfileRepository _doctorRepository;
        private readonly IOfficeApiClient _officeApiClient;

        public GetDoctorsForReceptionistQueryHandler(
            IDoctorProfileRepository doctorRepository,
            IOfficeApiClient officeApiClient)
        {
            _doctorRepository = doctorRepository;
            _officeApiClient = officeApiClient;
        }

        public async Task<List<DoctorListItemDto>> Handle(
            GetDoctorsForReceptionistQuery request,
            CancellationToken cancellationToken)
        {
            var query = _doctorRepository.Query();

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                var fullNameLower = request.FullName.Trim().ToLower();
                query = query.Where(d =>
                    ((d.FirstName ?? "") + " " + (d.LastName ?? "") + " " + (d.MiddleName ?? ""))
                        .ToLower()
                        .Contains(fullNameLower)
                );
            }

            if (request.Specialization.HasValue)
            {
                query = query.Where(d => d.Specialization == request.Specialization.Value);
            }

            if (request.OfficeId.HasValue)
            {
                query = query.Where(d => d.OfficeId == request.OfficeId.Value);
            }

            var doctors = await query.ToListAsync(cancellationToken);

            var doctorDtos = new List<DoctorListItemDto>();
            foreach (var doctor in doctors)
            {
                string officeName = string.Empty;
                if (doctor.OfficeId != Guid.Empty)
                {
                    var office = await _officeApiClient.GetOfficeAddressAsync(doctor.OfficeId, cancellationToken);
                    officeName = office?.Address ?? string.Empty;
                }

                doctorDtos.Add(new DoctorListItemDto
                {
                    Id = doctor.Id,
                    FullName = $"{doctor.FirstName} {doctor.LastName} {doctor.MiddleName}".Trim(),
                    Specialization = doctor.Specialization,
                    Status = doctor.Status,
                    DateOfBirth = doctor.DateOfBirth,
                    OfficeAddress = officeName
                });
            }

            return doctorDtos;
        }
    }
}
