using InnoClinic.Domain.Common.Enums;

namespace InnoClinic.Server.Application.DTOs;
public record UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public UserRole Role { get; set; }
}

