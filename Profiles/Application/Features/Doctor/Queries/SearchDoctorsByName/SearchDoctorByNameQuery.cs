using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByName
{
    public sealed class SearchDoctorByNameQuery : IRequest<List<DoctorProfileDto>>
    {
        public string Name { get; }

        public SearchDoctorByNameQuery(string name)
        {
            Name = name;
        }
    }
}