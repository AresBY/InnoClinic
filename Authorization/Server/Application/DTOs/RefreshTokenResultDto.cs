namespace InnoClinic.Authorization.Application.DTOs
{
    public record RefreshTokenResultDto
    {
        public bool IsSuccess { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
