using InnoClinic.Services.Domain.Enums;

namespace InnoClinic.Services.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public ServiceCategory Category { get; set; }
        public bool Status { get; set; }

        public string? Specialization { get; set; }
    }
}
