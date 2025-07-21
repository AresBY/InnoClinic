using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands
{
    public sealed class SignInCommand : IRequest<SignInResultDto>
    {
        public string Email { get; }
        public string Password { get; }

        public SignInCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
