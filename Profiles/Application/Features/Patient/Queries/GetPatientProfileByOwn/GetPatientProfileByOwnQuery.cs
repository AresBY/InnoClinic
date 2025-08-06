using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByOwn
{
    public class GetPatientProfileByOwnQuery : IRequest<PatientProfileDto>
    {
        public Guid OwnerId { get; }

        public GetPatientProfileByOwnQuery(Guid patientId)
        {
            OwnerId = patientId;
        }
    }
}
