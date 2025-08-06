using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByDoctor
{
    public class GetPatientProfileByDoctorQuery : IRequest<PatientProfileDto>
    {
        public Guid PatientId { get; }

        public GetPatientProfileByDoctorQuery(Guid patientId)
        {
            PatientId = patientId;
        }
    }
}
