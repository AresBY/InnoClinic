using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Application.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Guid userId, string email, UserRole role);
        string GenerateRefreshToken();
    }
}
