using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeByIdQuery : IRequest<OfficeDto>
    {
        public string Id { get; }

        public GetOfficeByIdQuery(string id)
        {
            Id = id;
        }
    }
}
