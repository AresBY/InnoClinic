using InnoClinic.Domain.Common.Enums;
using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ReEnteredPassword { get; set; } = default!;
    public UserRole Role { get; set; }
}
