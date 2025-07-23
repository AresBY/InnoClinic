using InnoClinic.Application.Resources;
using InnoClinic.Domain.Common.Enums;
using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Exceptions;
using InnoClinic.Server.Application.Interfaces;
using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InnoClinic.Server.Application.Features.Auth.Commands;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email, cancellationToken))
            throw new ConflictException(ValidationMessages.EmailAlreadyExists);

        User user = request.Role switch
        {
            UserRole.Doctor => new Doctor(),
            UserRole.Patient => new Patient(),
            _ => throw new ArgumentException($"Unsupported user role: {request.Role}")
        };

        user.Id = Guid.NewGuid();
        user.Email = request.Email;
        user.CreatedAt = DateTimeOffset.UtcNow;
        user.IsEmailConfirmed = false;

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        try
        {
            var addedPatientId = await _userRepository.AddAsync(user, cancellationToken);

            var frontendBaseUrl = _configuration["Frontend:BaseUrl"];
            var confirmationLink = $"{frontendBaseUrl}/confirm-email?userId={addedPatientId}";

            await _emailSender.SendEmailAsync(
                request.Email,
                subject: "Confirm your registration",
                body: $@"
                   <p>Click the link below to confirm your registration:</p>
                   <a href=""http://localhost:4200/confirm-email?userId={user.Id}"">
                       Confirm Email
                   </a>"
            );


            return new UserDto
            {
                Id = addedPatientId,
                Email = request.Email
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to register the patient.", ex);
        }
    }
}
