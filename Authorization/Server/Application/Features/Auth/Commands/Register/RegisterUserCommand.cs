using InnoClinic.Authorization.Domain.Common.Enums;
using InnoClinic.Authorization.Application.DTOs;
using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ReEnteredPassword { get; set; } = default!;
    public UserRole Role { get; set; }
}
