using InnoClinicCommon.Enums;

namespace InnoClinic.Profiles.Domain.Entities;

public class DoctorProfile : UserBaseProfile
{
    public string Specialization { get; set; } = null!;
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
    public string Email { get; set; } = null!;
    public DateTimeOffset DateOfBirth { get; set; }
}
