namespace InnoClinic.Authorization.Application.DTOs;
public record SignInResultDto
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public string? Message { get; init; }
    public Guid? UserId { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}

