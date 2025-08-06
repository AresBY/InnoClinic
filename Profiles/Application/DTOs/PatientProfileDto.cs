namespace InnoClinic.Profiles.Application.DTOs
{
    public record PatientProfileDto : BaseProfileDto
    {
        public string PhoneNumber { get; set; } = default!;
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
