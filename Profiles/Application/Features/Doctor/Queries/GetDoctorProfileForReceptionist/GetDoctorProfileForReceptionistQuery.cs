using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileForReceptionist
{
    public sealed class GetDoctorProfileForReceptionistQuery : IRequest<DoctorProfileDetailDto>
    {
        public Guid DoctorId { get; }

        public GetDoctorProfileForReceptionistQuery(Guid doctorId)
        {
            DoctorId = doctorId;
        }
    }
}
