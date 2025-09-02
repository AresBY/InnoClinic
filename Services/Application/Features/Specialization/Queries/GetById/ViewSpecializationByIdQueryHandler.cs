using InnoClinic.Services.Application.DTOs;
using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Queries.GetById
{
    public sealed class ViewSpecializationByIdQueryHandler
    : IRequestHandler<ViewSpecializationByIdQuery, ViewSpecializationResultDto>
    {
        private readonly ISpecializationRepository _repository;

        public ViewSpecializationByIdQueryHandler(ISpecializationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ViewSpecializationResultDto> Handle(
            ViewSpecializationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var specialization = await _repository
                .GetByIdWithServicesAsync(request.SpecializationId, cancellationToken);

            if (specialization is null)
                throw new NotFoundException($"Specialization {request.SpecializationId} not found");

            return new ViewSpecializationResultDto
            {
                Id = specialization.Id,
                Name = specialization.Name,
                IsActive = specialization.IsActive,
                Services = specialization.Services.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    IsActive = s.IsActive,
                    Category = s.Category
                }).ToList()
            };
        }
    }

}
