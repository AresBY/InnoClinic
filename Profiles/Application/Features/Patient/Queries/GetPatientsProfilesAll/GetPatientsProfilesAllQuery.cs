using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientsProfilesAll
{
    public class GetPatientsProfilesAllQuery : IRequest<List<PatientProfileDto>>
    {
    }
}
