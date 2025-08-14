namespace InnoClinic.Profiles.Application.DTOs
{
    public sealed class OfficeAddressDto
    {
        public Guid OfficeId { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string OfficeNumber { get; set; } = null!;
        public string Address => $"{City}, {Street} {HouseNumber} {OfficeNumber}";
    }
}
