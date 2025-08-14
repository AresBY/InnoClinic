namespace InnoClinic.Profiles.Application.DTOs
{
    public sealed record PatientListItemDto : BaseProfileDto
    {
        public DateTimeOffset DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = default!;

        public string? FullName { get; set; }
    }
}
