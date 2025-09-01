using InnoClinic.Services.Domain.Enums;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Commands.EditService
{
    public sealed record EditServiceCommand(
        Guid ServiceId,
        string Name,
        decimal Price,
        ServiceCategory Category,
        bool Status,
        Guid SpecializationId
    ) : IRequest<Unit>;
}
