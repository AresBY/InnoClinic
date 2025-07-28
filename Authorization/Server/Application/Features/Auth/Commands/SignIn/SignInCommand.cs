using InnoClinic.Authorization.Domain.Common.Enums;
using InnoClinic.Authorization.Application.DTOs;
using MediatR;
using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands
{
    public sealed class SignInCommand : IRequest<SignInResultDto>
    {
        public string Email { get; }
        public string Password { get; }
        public UserRole Role { get; }

        public SignInCommand(string email, string password, UserRole role)
        {
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
