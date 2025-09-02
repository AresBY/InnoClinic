namespace InnoClinic.Services.Application.DTOs
{
    public sealed class ViewSpecializationResultDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
        public List<ServiceDto> Services { get; init; } = new();
    }
}
