using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.JWT;

using InnoClinicCommon.JWT;

using MediatR;

using Microsoft.Extensions.Options;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResultDto>
    {
        private readonly IJwtTokenGenerator _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IJwtTokenGenerator tokenService,
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtOptions)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<RefreshTokenResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (user == null || user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new RefreshTokenResultDto
            {
                IsSuccess = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
