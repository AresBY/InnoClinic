using InnoClinic.Application.JWT;
using InnoClinic.Application.Resources;
using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Features.Auth.Commands;
using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace InnoClinic.Server.Application.Features.Auth.Commands;

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

        if (user == null)
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Email);

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
