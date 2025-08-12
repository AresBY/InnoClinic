using InnoClinicCommon.Enums;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.ChangeDoctorStatus
{
    public sealed class ChangeDoctorStatusCommand : IRequest<Unit>
    {
        public Guid DoctorId { get; set; }
        public DoctorStatus Status { get; set; }
    }
}
