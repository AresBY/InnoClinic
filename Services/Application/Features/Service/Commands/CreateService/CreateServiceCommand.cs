using InnoClinic.Services.Domain.Enums;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Commands.CreateService
{
    public sealed class CreateServiceCommand : IRequest<Guid>
    {
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
        public ServiceCategory Category { get; init; }
        public bool Status { get; init; } = false;
    }
}
