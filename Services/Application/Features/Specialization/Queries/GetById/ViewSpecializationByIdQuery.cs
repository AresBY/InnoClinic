using InnoClinic.Services.Application.DTOs;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Queries.GetById
{
    public sealed record ViewSpecializationByIdQuery(Guid SpecializationId)
       : IRequest<ViewSpecializationResultDto>;
}
