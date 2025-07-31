using InnoClinic.Offices.Application.DTOs;
using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries
{
    public class GetAllQuery : IRequest<List<OfficeDto>>
    {
    }
}
