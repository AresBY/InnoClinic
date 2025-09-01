using InnoClinic.Services.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommandHandler
        : IRequestHandler<CreateSpecializationCommand, Guid>
    {
        private readonly ISpecializationRepository _repository;

        public CreateSpecializationCommandHandler(ISpecializationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization = new InnoClinic.Specializations.Domain.Entities.Specialization
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Services = request.Services.Select(s => new Domain.Entities.Service
                {
                    Name = s.Name,
                    Price = s.Price,
                    IsActive = s.IsActive,
                    Category = s.Category
                }).ToList()
            };

            await _repository.AddAsync(specialization, cancellationToken);

            return specialization.Id;
        }
    }
}
