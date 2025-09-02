namespace InnoClinic.Services.Application.DTOs
{
    public sealed class ViewSpecializationListItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}
