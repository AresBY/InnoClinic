using InnoClinic.Services.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Commands.CreateService
{
    public sealed class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Guid>
    {
        private readonly IServiceRepository _serviceRepository;

        public CreateServiceCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Guid> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Domain.Entities.Service
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Category = request.Category,
                Status = request.Status
            };

            await _serviceRepository.AddAsync(service, cancellationToken);

            return service.Id;
        }
    }
}
