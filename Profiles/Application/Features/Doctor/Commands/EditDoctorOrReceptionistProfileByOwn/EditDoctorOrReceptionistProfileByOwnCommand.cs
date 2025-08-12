using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Domain.Enums;

using InnoClinicCommon.Enums;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.EditDoctorOrReceptionistProfileByOwn
{
    public class EditDoctorOrReceptionistProfileByOwnCommand : IRequest<DoctorProfileDto>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DoctorSpecialization Specialization { get; set; }

        public Guid OfficeId { get; set; }

        public int CareerStartYear { get; set; }

        public DoctorStatus Status { get; set; }
    }
}
