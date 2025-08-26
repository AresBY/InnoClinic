using InnoClinic.Services.Domain.Enums;

namespace InnoClinic.Services.Application.DTOs
{
    public sealed class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public ServiceCategory Category { get; set; }
        public string? Specialization { get; set; }
    }
}
