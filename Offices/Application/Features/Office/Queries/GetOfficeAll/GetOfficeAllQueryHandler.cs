using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOfficeAll
{
    public class GetOfficeAllQueryHandler : IRequestHandler<GetOfficeAllQuery, List<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficeAllQueryHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<List<OfficeDto>> Handle(GetOfficeAllQuery request, CancellationToken cancellationToken)
        {
            var offices = await _officeRepository.GetAllAsync(cancellationToken);

            return offices.Select(o => o.ToDto()).ToList();
        }
    }
}
