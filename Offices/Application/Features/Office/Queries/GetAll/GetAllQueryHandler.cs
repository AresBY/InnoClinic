using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InnoClinic.Offices.Application.Features.Office.Queries
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetAllQueryHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<List<OfficeDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var offices = await _officeRepository.GetAllAsync(cancellationToken);

            return offices.Select(o => o.ToDto()).ToList();
        }
    }
}
