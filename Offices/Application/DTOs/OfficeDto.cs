namespace InnoClinic.Offices.Application.DTOs
{
    public record OfficeDto
    {
        public string Id { get; init; } = default!;
        public string? PhotoUrl { get; init; }
        public string City { get; init; } = default!;
        public string Street { get; init; } = default!;
        public string HouseNumber { get; init; } = default!;
        public string? OfficeNumber { get; init; }
        public bool Status { get; init; }
        public string RegistryPhoneNumber { get; init; } = default!;
    }
}