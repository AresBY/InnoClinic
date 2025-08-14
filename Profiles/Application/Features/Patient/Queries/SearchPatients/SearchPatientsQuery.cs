using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.SearchPatients
{
    public sealed class SearchPatientsQuery : IRequest<List<PatientListItemDto>>
    {
        public string? FullName { get; set; }
    }
}
