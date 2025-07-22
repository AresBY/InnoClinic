using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands;

public class RegisterPatientCommand : IRequest<PatientDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ReEnteredPassword { get; set; } = default!;
}
