using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetPatientProfile
{
    public class GetPatientProfileQuery : IRequest<PatientProfileDto>
    {
        public Guid OwnerId { get; }

        public GetPatientProfileQuery(Guid patientId)
        {
            OwnerId = patientId;
        }
    }
}
