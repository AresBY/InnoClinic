namespace InnoClinic.Offices.Application.DTOs
{
    public class OfficeMapDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
