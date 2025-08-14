using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Queries.GetReceptionistById
{
    public sealed class GetReceptionistByIdQuery : IRequest<ReceptionistProfileDto?>
    {
        public Guid ProfileId { get; set; }
    }
}
