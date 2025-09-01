using InnoClinic.Services.Application.DTOs;
using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Domain.Enums;

using MediatR;

namespace InnoClinic.Services.Application.Features.Service.Queries.ViewServices
{
    public sealed class ViewServicesQueryHandler : IRequestHandler<ViewServicesQuery, ViewServicesResultDto>
    {
        private readonly IServiceRepository _repository;

        public ViewServicesQueryHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ViewServicesResultDto> Handle(ViewServicesQuery request, CancellationToken cancellationToken)
        {
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

            return result;
        }
    }
}
