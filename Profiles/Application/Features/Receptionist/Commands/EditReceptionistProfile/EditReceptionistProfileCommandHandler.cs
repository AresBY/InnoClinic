using InnoClinic.Profiles.Application.Interfaces.Repositories;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.EditReceptionistProfile
{
    public sealed class EditReceptionistProfileCommandHandler : IRequestHandler<EditReceptionistProfileCommand, Guid>
    {
        private readonly IReceptionistProfileRepository _receptionistRepository;

        public EditReceptionistProfileCommandHandler(IReceptionistProfileRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }

        public async Task<Guid> Handle(EditReceptionistProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _receptionistRepository.GetByIdAsync(request.ProfileId, cancellationToken);
            if (profile == null)
                throw new NotFoundException($"Receptionist profile with  {request.ProfileId} not found");

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.MiddleName = request.MiddleName;
            profile.OfficeId = request.OfficeId;

            await _receptionistRepository.UpdateAsync(profile, cancellationToken);

            return profile.Id;
        }
    }
}
