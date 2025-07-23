using InnoClinic.Domain.Common.Enums;
using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands
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
