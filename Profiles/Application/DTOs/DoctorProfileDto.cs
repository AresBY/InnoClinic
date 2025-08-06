using InnoClinicCommon.Enums;

namespace InnoClinic.Offices.Application.DTOs
{
    public record DoctorProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public Guid OfficeId { get; set; }
        public int CareerStartYear { get; set; }
        public DoctorStatus Status { get; set; }
    }
}
