using InnoClinic.Offices.Domain.Enums;

using InnoClinicCommon.Enums;

namespace InnoClinic.Profiles.Domain.Entities;

public sealed class DoctorProfile : UserBaseProfile
{
    public DoctorSpecialization Specialization { get; set; } = DoctorSpecialization.None;
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
    public string Email { get; set; } = null!;
    public DateTimeOffset DateOfBirth { get; set; }
}
