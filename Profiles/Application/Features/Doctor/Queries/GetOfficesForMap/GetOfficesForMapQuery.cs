using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetOfficesForMapFromApi
{
    public sealed record GetOfficesForMapFromApiQuery() : IRequest<List<OfficeMapDto>>;
}
