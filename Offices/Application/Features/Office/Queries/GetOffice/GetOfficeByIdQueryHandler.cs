using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeByIdQueryHandler : IRequestHandler<GetOfficeByIdQuery, OfficeDto>
    {
        private readonly IOfficeRepository _repository;

        public GetOfficeByIdQueryHandler(IOfficeRepository repository)
        {
            _repository = repository;
        }

        public async Task<OfficeDto> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
        {
            var office = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (office == null)
                throw new NotFoundException($"Office with Id {request.Id} not found.");

            return office.ToDto();
        }
    }
}
