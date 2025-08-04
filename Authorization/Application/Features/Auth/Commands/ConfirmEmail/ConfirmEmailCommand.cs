using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }
}
