using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfileByReceptionist
{
    public sealed class CreatePatientProfileByReceptionistCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
