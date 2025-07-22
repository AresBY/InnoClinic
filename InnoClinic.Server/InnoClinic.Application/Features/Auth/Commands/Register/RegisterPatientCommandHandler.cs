using InnoClinic.Application.Resources;
using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Exceptions;
using InnoClinic.Server.Application.Interfaces;
using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InnoClinic.Server.Application.Features.Auth.Commands;
public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, PatientDto>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher<Patient> _passwordHasher;
    private readonly IConfiguration _configuration;

    public RegisterPatientCommandHandler(
        IPatientRepository patientRepository,
        IEmailSender emailSender,
        IPasswordHasher<Patient> passwordHasher,
        IConfiguration configuration)
    {
        _patientRepository = patientRepository;
        _emailSender = emailSender;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<PatientDto> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
        if (await _patientRepository.ExistsAsync(request.Email, cancellationToken))
            throw new ConflictException(ValidationMessages.EmailAlreadyExists);

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            CreatedAt = DateTimeOffset.UtcNow,
            IsEmailConfirmed = false
        };

        patient.PasswordHash = _passwordHasher.HashPassword(patient, request.Password);

        try
        {
            var addedPatientId = await _patientRepository.AddAsync(patient, cancellationToken);

            var frontendBaseUrl = _configuration["Frontend:BaseUrl"];
            var confirmationLink = $"{frontendBaseUrl}/confirm-email?userId={addedPatientId}";

            await _emailSender.SendEmailAsync(
                request.Email,
                subject: "Confirm your registration",
                body: $@"
                   <p>Click the link below to confirm your registration:</p>
                   <a href=""http://localhost:4200/confirm-email?userId={patient.Id}"">
                       Confirm Email
                   </a>"
            );


            return new PatientDto
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
