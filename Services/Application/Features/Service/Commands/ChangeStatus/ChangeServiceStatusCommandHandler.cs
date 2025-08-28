using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Services.Application.Features.Services.Commands.ChangeServiceStatus
{
    public sealed class ChangeServiceStatusCommandHandler : IRequestHandler<ChangeServiceStatusCommand, Unit>
    {
        private readonly IServiceRepository _repository;

        public ChangeServiceStatusCommandHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ChangeServiceStatusCommand request, CancellationToken cancellationToken)
        {
            var service = await _repository.GetByIdAsync(request.ServiceId, cancellationToken);
            if (service is null)
                throw new NotFoundException($"Service with id {request.ServiceId} not found");

            service.Status = request.Status;

            await _repository.UpdateStatusOnlyAsync(service, cancellationToken);

            return Unit.Value;
        }
    }
}
