using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOfficeAll
{
    public class GetOfficeAllQuery : IRequest<List<OfficeDto>>
    {
    }
}
