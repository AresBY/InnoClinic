using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Commands.EditService
{
    public sealed class EditServiceCommandHandler : IRequestHandler<EditServiceCommand, Unit>
    {
        private readonly IServiceRepository _repository;

        public EditServiceCommandHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(EditServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _repository.GetByIdAsync(request.ServiceId, cancellationToken);
            if (service is null)
                throw new NotFoundException($"Service with id {request.ServiceId} not found");

            service.Name = request.Name;
            service.Price = request.Price;
            service.Category = request.Category;
            service.IsActive = request.Status;
            service.SpecializationId = request.SpecializationId;

            await _repository.UpdateAsync(service, cancellationToken);

            return Unit.Value;
        }
    }
}
