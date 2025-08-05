using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.JWT;
using InnoClinic.Authorization.Application.Resources;
using InnoClinic.Authorization.Domain.Entities;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignInCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<SignInResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };
        }

        if (user is Doctor doctor)
        {
            var allowedStatuses = new[]
            {
                DoctorStatus.AtWork,
                DoctorStatus.OnVacation,
                DoctorStatus.SickDay,
                DoctorStatus.SickLeave,
                DoctorStatus.SelfIsolation,
                DoctorStatus.LeaveWithoutPay
            };

            if (!allowedStatuses.Contains(doctor.WorkerStatus))
            {
                return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };
            }
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Email, user.Role);

        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new SignInResultDto
        {
            IsSuccess = true,
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Message = "You've signed in successfully",
        };
    }
}
