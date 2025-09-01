using InnoClinic.Services.Application.DTOs;
using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Queries.ViewService
{
    public sealed class ViewServiceQueryHandler : IRequestHandler<ViewServiceQuery, ViewServiceResultDto>
    {
        private readonly IServiceRepository _repository;

        public ViewServiceQueryHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ViewServiceResultDto> Handle(ViewServiceQuery request, CancellationToken cancellationToken)
        {
            var service = await _repository.GetByIdAsync(request.ServiceId, cancellationToken);
            if (service is null)
                throw new NotFoundException($"Service with id {request.ServiceId} not found");

            return new ViewServiceResultDto(
                service.Id,
                service.Name,
                service.Price,
                service.Category,
                service.IsActive
            );
        }
    }
}
