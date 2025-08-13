using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileForReceptionist
{
    public sealed class GetDoctorProfileForReceptionistQueryHandler
        : IRequestHandler<GetDoctorProfileForReceptionistQuery, DoctorProfileDetailDto?>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public GetDoctorProfileForReceptionistQueryHandler(
            IDoctorProfileRepository doctorRepository,
            IOfficeApiClient officeApiClient)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorProfileDetailDto?> Handle(GetDoctorProfileForReceptionistQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.Query()
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);

            if (doctor == null) return null;

            return new DoctorProfileDetailDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                MiddleName = doctor.MiddleName,
                Email = doctor.Email,
                DateOfBirth = doctor.DateOfBirth,
                Specialization = doctor.Specialization,
                OfficeId = doctor.OfficeId,
                CareerStartYear = doctor.CareerStartYear,
                Status = doctor.Status
            };
        }
    }
}
