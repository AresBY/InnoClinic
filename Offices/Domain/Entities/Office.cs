namespace InnoClinic.Offices.Domain.Entities
{
    public class Office
    {
        public string Id { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; } = default!;
        public bool Status { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
