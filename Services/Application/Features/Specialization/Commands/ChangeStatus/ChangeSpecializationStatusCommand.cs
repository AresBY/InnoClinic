using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Commands.ChangeStatus
{
    public sealed class ChangeSpecializationStatusCommand : IRequest<Unit>
    {
        public Guid SpecializationId { get; set; }
        public bool IsActive { get; set; }
    }
}
