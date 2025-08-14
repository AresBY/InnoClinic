namespace InnoClinic.Profiles.Application.DTOs
{
    public record class ReceptionistProfileDto : BaseProfileDto
    {
        public string OfficeAddress { get; set; } = null!;
    }
}
