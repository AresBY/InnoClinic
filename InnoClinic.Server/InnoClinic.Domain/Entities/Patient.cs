namespace InnoClinic.Server.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsEmailConfirmed { get; set; }
}

