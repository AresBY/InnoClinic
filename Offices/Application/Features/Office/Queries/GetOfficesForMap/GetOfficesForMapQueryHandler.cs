using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOfficesForMap
{
    public sealed class GetOfficesForMapQueryHandler : IRequestHandler<GetOfficesForMapQuery, List<OfficeMapDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficesForMapQueryHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<List<OfficeMapDto>> Handle(GetOfficesForMapQuery request, CancellationToken cancellationToken)
        {
            var offices = await _officeRepository.GetAllAsync(cancellationToken);

            var result = offices.Select(o => o.ToMapDto()).ToList();

            return result;
        }
    }
}
