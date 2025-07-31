using MediatR;
using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;
using InnoClinicCommon.Exception;

namespace InnoClinic.Offices.Application.Features.Office.Queries
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, OfficeDto>
    {
        private readonly IOfficeRepository _repository;

        public GetByIdQueryHandler(IOfficeRepository repository)
        {
            _repository = repository;
        }

        public async Task<OfficeDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var office = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (office == null)
                throw new NotFoundException($"Office with Id {request.Id} not found.");

            return office.ToDto();
        }
    }
}
