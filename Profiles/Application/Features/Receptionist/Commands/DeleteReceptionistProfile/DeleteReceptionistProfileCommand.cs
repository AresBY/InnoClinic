using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.DeleteReceptionistProfile
{
    public sealed class DeleteReceptionistProfileCommand : IRequest<Unit>
    {
        public Guid ProfileId { get; set; }
    }
}
