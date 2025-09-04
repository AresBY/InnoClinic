using InnoClinic.Services.Application.DTOs;
using InnoClinic.Services.Application.Interfaces.Caching;
using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Application.Resources;
using InnoClinic.Services.Domain.Enums;

using MediatR;

namespace InnoClinic.Services.Application.Features.Service.Queries.ViewServices
{
    public sealed class ViewServicesQueryHandler : IRequestHandler<ViewServicesQuery, ViewServicesResultDto>
    {
        private readonly IServiceRepository _repository;
        private readonly ICacheService _cache;

        public ViewServicesQueryHandler(IServiceRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<ViewServicesResultDto> Handle(ViewServicesQuery request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<ViewServicesResultDto>(CacheKeys.Services, cancellationToken);
            if (cached is not null)
            {
                return cached;
            }

            var result = new ViewServicesResultDto();

            var consultations = await _repository.GetActiveServicesByCategoryAsync(ServiceCategory.Consultation, cancellationToken);

            var groupedConsultations = consultations
                .GroupBy(s => s.Specialization?.Name ?? "Unknown")
                .Select(g => new ConsultationGroupDto
                {
                    Specialization = g.Key,
                    Services = g.Select(s => new ServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.Price,
                        Category = s.Category
                    }).ToList()
                })
                .ToList();

            result.Consultations = groupedConsultations;

            var diagnostics = await _repository.GetActiveServicesByCategoryAsync(ServiceCategory.Diagnostics, cancellationToken);
            result.Diagnostics = diagnostics.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Category = s.Category
            }).ToList();

            var analyses = await _repository.GetActiveServicesByCategoryAsync(ServiceCategory.Analyses, cancellationToken);
            result.Analyses = analyses.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Category = s.Category
            }).ToList();

            await _cache.SetAsync(CacheKeys.Services, result, TimeSpan.FromMinutes(1), cancellationToken);

            return result;
        }
    }
}
