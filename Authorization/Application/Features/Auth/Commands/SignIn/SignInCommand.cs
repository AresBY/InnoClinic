using InnoClinic.Authorization.Domain.Common.Enums;
using InnoClinic.Authorization.Application.DTOs;
using MediatR;
using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands
{
    public sealed class SignInCommand : IRequest<SignInResultDto>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.None;
    }
}
