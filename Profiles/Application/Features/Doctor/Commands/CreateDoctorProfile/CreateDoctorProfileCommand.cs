using InnoClinic.Offices.Domain.Enums;

using InnoClinicCommon.Enums;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile
{
    public sealed class CreateDoctorProfileCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public DoctorSpecialization Specialization { get; set; }
        public Guid OfficeId { get; set; }

        public int CareerStartYear { get; set; }

        public DoctorStatus Status { get; set; }

        public Guid OwnerId { get; set; }
    }
}
