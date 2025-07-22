namespace InnoClinic.Server.Application.Common.Settings
{
    public record JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
    }
}
