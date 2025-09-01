using InnoClinic.Services.Application.DTOs;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommand : IRequest<Guid>
    {
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
        public List<ServiceDto> Services { get; init; } = new();
    }
}
