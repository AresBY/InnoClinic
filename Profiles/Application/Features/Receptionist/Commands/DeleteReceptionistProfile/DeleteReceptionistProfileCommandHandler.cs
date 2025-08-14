using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.DeleteReceptionistProfile
{
    public sealed class DeleteReceptionistProfileCommandHandler : IRequestHandler<DeleteReceptionistProfileCommand, Unit>
    {
        private readonly IReceptionistProfileRepository _repository;

        public DeleteReceptionistProfileCommandHandler(IReceptionistProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteReceptionistProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.ProfileId, cancellationToken);
            if (profile == null)
                throw new KeyNotFoundException($"Receptionist with Id {request.ProfileId} not found.");

            await _repository.DeleteAsync(profile, cancellationToken);

            return Unit.Value;
        }
    }
}
