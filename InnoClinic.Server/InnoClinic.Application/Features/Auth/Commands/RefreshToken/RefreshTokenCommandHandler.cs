using InnoClinic.Application.DTOs;
using InnoClinic.Application.JWT;
using InnoClinic.Server.Application.Common.Settings;
using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace InnoClinic.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResultDto>
    {
        private readonly IJwtTokenGenerator _tokenService;
        private readonly IPatientRepository _patientRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IJwtTokenGenerator tokenService,
            IPatientRepository patientRepository,
            IOptions<JwtSettings> jwtOptions)
        {
            _tokenService = tokenService;
            _patientRepository = patientRepository;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<RefreshTokenResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (patient == null || patient.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(patient.Id, patient.Email);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            patient.RefreshToken = newRefreshToken;
            patient.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

            await _patientRepository.UpdateAsync(patient, cancellationToken);

            return new RefreshTokenResultDto
            {
                IsSuccess = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
