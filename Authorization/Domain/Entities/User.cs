using InnoClinic.Authorization.Domain.Common.Enums;
using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;

public abstract class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public UserRole Role { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
}

