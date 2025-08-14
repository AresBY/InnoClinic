using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Queries.GetReceptionistById
{
    public sealed class GetReceptionistByIdQueryHandler : IRequestHandler<GetReceptionistByIdQuery, ReceptionistProfileDto?>
    {
        private readonly IReceptionistProfileRepository _repository;

        public GetReceptionistByIdQueryHandler(IReceptionistProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReceptionistProfileDto?> Handle(GetReceptionistByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.ProfileId, cancellationToken);

            if (profile == null)
                throw new NotFoundException($"Receptionist with Id {request.ProfileId} not found.");

            return new ReceptionistProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName
            };
        }
    }
}
