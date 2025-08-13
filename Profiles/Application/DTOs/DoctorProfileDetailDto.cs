using InnoClinic.Offices.Domain.Enums;

using InnoClinicCommon.Enums;

namespace InnoClinic.Profiles.Application.DTOs
{
    public record DoctorProfileDetailDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string Email { get; set; } = null!;
        public DateTimeOffset DateOfBirth { get; set; }
        public DoctorSpecialization Specialization { get; set; }
        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; } = null!;
        public int CareerStartYear { get; set; }
        public DoctorStatus Status { get; set; }
        public string PhotoUrl { get; set; } = null!;
    }
}
