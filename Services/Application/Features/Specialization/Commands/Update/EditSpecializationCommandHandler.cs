using InnoClinic.Services.Application.Features.Specialization.Commands.Update;
using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specializations.Commands.Update
{
    public sealed class EditSpecializationCommandHandler
        : IRequestHandler<EditSpecializationCommand, Unit>
    {
        private readonly ISpecializationRepository _repository;

        public EditSpecializationCommandHandler(ISpecializationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(EditSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization = await _repository.GetByIdWithServicesAsync(request.Id, cancellationToken);

            if (specialization is null)
                throw new NotFoundException($"Specialization {request.Id} not found");

            specialization.Name = request.Name;
            specialization.IsActive = request.IsActive;

            specialization.Services.Clear();

            foreach (var s in request.Services)
            {
                specialization.Services.Add(new Domain.Entities.Service
                {
                    Id = s.Id ?? Guid.NewGuid(),
                    Name = s.Name,
                    Price = s.Price,
                    Category = s.Category,
                    IsActive = s.IsActive,
                    SpecializationId = specialization.Id
                });
            }

            await _repository.UpdateAsync(specialization, cancellationToken);

            return Unit.Value;
        }
    }
}
