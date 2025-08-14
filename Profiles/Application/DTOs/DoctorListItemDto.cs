using InnoClinic.Offices.Domain.Enums;

using InnoClinicCommon.Enums;

namespace InnoClinic.Profiles.Application.DTOs
{
    public sealed class DoctorListItemDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public DoctorSpecialization Specialization { get; set; }
        public DoctorStatus Status { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string OfficeAddress { get; set; } = "Unknown";
    }
}
