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
    private readonly IPatientRepository _patientRepository;
    private readonly IPasswordHasher<Patient> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignInCommandHandler(IPatientRepository patientRepository, IPasswordHasher<Patient> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _patientRepository = patientRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<SignInResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (patient == null)
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };

        var verificationResult = _passwordHasher.VerifyHashedPassword(patient, patient.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return new SignInResultDto { IsSuccess = false, ErrorMessage = ErrorMessages.SignInFailedMessage };

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(patient.Id, patient.Email);

        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        patient.RefreshToken = refreshToken;

        await _patientRepository.UpdateAsync(patient, cancellationToken);

        return new SignInResultDto
        {
            IsSuccess = true,
            UserId = patient.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Message = "You've signed in successfully",
        };
    }
}
