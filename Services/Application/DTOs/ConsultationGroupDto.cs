namespace InnoClinic.Services.Application.DTOs
{
    public sealed class ConsultationGroupDto
    {
        public string Specialization { get; set; } = null!;
        public List<ServiceDto> Services { get; set; } = new();
    }
}
