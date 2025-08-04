using InnoClinic.Authorization.Application.DTOs;

using InnoClinicCommon.Enums;

using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ReEnteredPassword { get; set; } = default!;
    public UserRole Role { get; set; }
}
