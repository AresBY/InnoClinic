using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.DeletePatientProfile
{
    public sealed class DeletePatientProfileCommand : IRequest<Unit>
    {
        public Guid PatientId { get; set; }

        public DeletePatientProfileCommand(Guid patientId)
        {
            PatientId = patientId;
        }
    }
}
