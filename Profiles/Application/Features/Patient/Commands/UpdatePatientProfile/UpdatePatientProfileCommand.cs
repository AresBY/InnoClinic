using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.UpdatePatientProfile
{
    public sealed class UpdatePatientProfileCommand : IRequest<Unit>
    {
        public Guid PatientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}
