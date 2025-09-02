using InnoClinic.Services.Application.DTOs;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Queries.GetAll
{
    public sealed record ViewSpecializationsListQuery
       : IRequest<List<ViewSpecializationListItemDto>>;
}
