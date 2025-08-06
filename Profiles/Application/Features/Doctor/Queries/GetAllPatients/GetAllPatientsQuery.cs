using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllPatients
{
    public class GetAllPatientsQuery : IRequest<List<PatientProfileDto>>
    {
    }
}
