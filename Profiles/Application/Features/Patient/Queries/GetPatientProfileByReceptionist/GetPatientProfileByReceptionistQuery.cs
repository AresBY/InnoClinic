using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByReceptionist
{
    public sealed class GetPatientProfileByReceptionistQuery : IRequest<PatientProfileDto?>
    {
        public Guid PatientId { get; set; }
    }
}
