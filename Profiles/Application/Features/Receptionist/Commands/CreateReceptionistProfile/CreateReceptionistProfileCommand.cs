using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.CreateReceptionistProfile
{
    public sealed class CreateReceptionistProfileCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public Guid OwnerId { get; set; }
        public Guid OfficeId { get; set; }
    }
}
