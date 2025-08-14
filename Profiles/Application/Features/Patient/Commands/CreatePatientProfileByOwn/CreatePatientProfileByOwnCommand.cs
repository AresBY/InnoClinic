using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile
{
    public sealed class CreatePatientProfileByOwnCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public Guid OwnerId { get; set; }
    }
}
