using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOfficesForMap
{
    public sealed record GetOfficesForMapQuery() : IRequest<List<OfficeMapDto>>;
}
