namespace InnoClinic.Server.Application.DTOs;
public record PatientDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
}

