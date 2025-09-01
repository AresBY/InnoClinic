using InnoClinic.Services.Domain.Enums;
using InnoClinic.Specializations.Domain.Entities;

namespace InnoClinic.Services.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public ServiceCategory Category { get; set; }
        public bool IsActive { get; set; }
        public Guid SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
    }
}
