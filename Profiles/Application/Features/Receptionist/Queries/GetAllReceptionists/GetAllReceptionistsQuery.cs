using InnoClinic.Profiles.Application.DTOs;

using MediatR;


namespace InnoClinic.Profiles.Application.Features.Receptionist.Queries
{
    public sealed class GetAllReceptionistsQuery : IRequest<List<ReceptionistProfileDto>>
    {
    }
}
