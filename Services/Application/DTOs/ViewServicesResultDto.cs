namespace InnoClinic.Services.Application.DTOs
{
    public sealed class ViewServicesResultDto
    {
        public List<ConsultationGroupDto> Consultations { get; set; } = new();
        public List<ServiceDto> Diagnostics { get; set; } = new();
        public List<ServiceDto> Analyses { get; set; } = new();
    }

}
