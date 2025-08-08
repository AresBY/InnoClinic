using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileByOwn
{
    public sealed class GetDoctorProfileByOwnQuery : IRequest<DoctorProfileDto>
    {
        public Guid OwnerId { get; }

        public GetDoctorProfileByOwnQuery(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
