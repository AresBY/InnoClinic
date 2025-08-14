using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.EditReceptionistProfile
{
    public sealed class EditReceptionistProfileCommand : IRequest<Guid>
    {
        public Guid ProfileId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public Guid OfficeId { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
