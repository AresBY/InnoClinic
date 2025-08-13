using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByNameForAdmin
{
    public sealed class SearchDoctorByNameForAdminQuery : IRequest<List<DoctorProfileDto>>
    {
        public string Name { get; }

        public SearchDoctorByNameForAdminQuery(string name)
        {
            Name = name;
        }
    }
}
