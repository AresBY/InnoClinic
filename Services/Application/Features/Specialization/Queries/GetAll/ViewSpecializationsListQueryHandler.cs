namespace InnoClinic.Services.Application.Features.Specialization.Queries.GetAll
{
    using global::InnoClinic.Services.Application.DTOs;
    using global::InnoClinic.Services.Application.Interfaces.Repositories;

    using MediatR;

    namespace InnoClinic.Services.Application.Features.Specializations.Queries.ViewSpecializationsList
    {
        public sealed class ViewSpecializationsListQueryHandler
            : IRequestHandler<ViewSpecializationsListQuery, List<ViewSpecializationListItemDto>>
        {
            private readonly ISpecializationRepository _repository;

            public ViewSpecializationsListQueryHandler(ISpecializationRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ViewSpecializationListItemDto>> Handle(
                ViewSpecializationsListQuery request,
                CancellationToken cancellationToken)
            {
                var specializations = await _repository.GetAllAsync(cancellationToken);

                return specializations.Select(s => new ViewSpecializationListItemDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActive = s.IsActive
                }).ToList();
            }
        }
    }

}
