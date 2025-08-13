using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsByFilter
{
    public sealed class GetDoctorsByFilterQuery : IRequest<List<DoctorProfileDto>>
    {
        public Guid? OfficeId { get; }

        public GetDoctorsByFilterQuery(Guid? officeId)
        {
            OfficeId = officeId;
        }
    }
}
