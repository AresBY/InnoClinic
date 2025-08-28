using InnoClinic.Services.Application.DTOs;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Queries.ViewService
{
    public sealed record ViewServiceQuery(Guid ServiceId) : IRequest<ViewServiceResultDto>;
}
