using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Application.Interfaces;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.Resources;
using InnoClinic.Authorization.Domain.Entities;

using InnoClinicCommon.Enums;
using InnoClinicCommon.Exception;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using static InnoClinicCommon.DTOs.RegisterPatientEntities;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.RegisterUser;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration,
        IPublishEndpoint publishEndpoint)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email, cancellationToken))
            throw new ConflictException(ValidationMessages.EmailAlreadyExists);

        User user = request.Role switch
        {
            UserRole.Doctor => new Doctor(),
            UserRole.Receptionist => new Receptionist(),
            UserRole.Admin => new Admin(),
            UserRole.Patient => new Patient(),
            _ => throw new ArgumentException($"Unsupported user role: {request.Role}")
        };

        user.Id = Guid.NewGuid();
        user.Email = request.Email;
        user.CreatedAt = DateTimeOffset.UtcNow;
        user.IsEmailConfirmed = false;
        user.Role = request.Role;
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
                   <a href=""{confirmationLink}"">
                       Confirm Email
                   </a>"
            );

            if (request.Role == UserRole.Patient)
            {
                await _publishEndpoint.Publish<RegisterPatient>(new
                {
                    CorrelationId = Guid.NewGuid(),
                    Email = request.Email
                }, cancellationToken);
            }

            return new UserDto
            {
                Id = addedPatientId,
                Email = request.Email,
                Role = request.Role
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to register the patient.", ex);
        }
    }
}
