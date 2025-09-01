using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Specializations.Domain.Entities
{
    public class Specialization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public List<Service> Services { get; set; } = new();
    }
}
