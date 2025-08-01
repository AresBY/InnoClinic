using MediatR;
using InnoClinic.Offices.Application.DTOs;

namespace InnoClinic.Offices.Application.Features.Office.Queries
{
    public class GetByIdQuery : IRequest<OfficeDto>
    {
        public string Id { get; }

        public GetByIdQuery(string id)
        {
            Id = id;
        }
    }
}
