using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsByOfficeForReceptionist
{
    public sealed class GetDoctorsByOfficeForReceptionistQuery : IRequest<List<DoctorProfileDto>>
    {
        public Guid? OfficeId { get; }

        public GetDoctorsByOfficeForReceptionistQuery(Guid? officeId)
        {
            OfficeId = officeId;
        }
    }
}
