using InnoClinic.Offices.Domain.Enums;
using InnoClinic.Profiles.Application.DTOs;

using InnoClinicCommon.Enums;

namespace InnoClinic.Offices.Application.DTOs
{
    public record DoctorProfileDto : BaseProfileDto
    {
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public DoctorSpecialization Specialization { get; set; }
        public Guid OfficeId { get; set; }
        public int CareerStartYear { get; set; }
        public DoctorStatus Status { get; set; }

        public Guid OwnerId { get; set; }
    }
}
