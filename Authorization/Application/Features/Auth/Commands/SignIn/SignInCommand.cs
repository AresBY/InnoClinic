using InnoClinic.Authorization.Application.DTOs;

using InnoClinicCommon.Enums;

using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.SignIn
{
    public sealed class SignInCommand : IRequest<SignInResultDto>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.None;
    }
}
