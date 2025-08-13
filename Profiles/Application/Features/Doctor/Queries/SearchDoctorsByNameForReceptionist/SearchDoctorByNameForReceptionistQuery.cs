using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByNameForReceptionist
{
    public sealed class SearchDoctorByNameForReceptionistQuery : IRequest<List<DoctorProfileDto>>
    {
        public string Name { get; }

        public SearchDoctorByNameForReceptionistQuery(string name)
        {
            Name = name;
        }
    }
}
