namespace InnoClinic.Offices.Domain.Entities
{
    public class Office
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; } = default!;
        public bool Status { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
