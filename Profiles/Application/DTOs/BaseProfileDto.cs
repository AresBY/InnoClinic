namespace InnoClinic.Profiles.Application.DTOs
{
    public record BaseProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? MiddleName { get; set; }
    }
}
