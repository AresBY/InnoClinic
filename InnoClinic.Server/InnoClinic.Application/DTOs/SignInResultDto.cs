namespace InnoClinic.Server.Application.DTOs;
public record SignInResultDto
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public Guid? UserId { get; init; }
}

