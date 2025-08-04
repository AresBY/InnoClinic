using InnoClinic.Authorization.Application.DTOs;

using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResultDto>
    {
        public string RefreshToken { get; set; } = default!;
    }
}
