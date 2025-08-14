using InnoClinic.Profiles.Application.Features.Receptionist.Commands.CreateReceptionistProfile;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using MediatR;

public sealed class CreateReceptionistProfileCommandHandler : IRequestHandler<CreateReceptionistProfileCommand, Guid>
{
    private readonly IReceptionistProfileRepository _receptionistRepository;

    public CreateReceptionistProfileCommandHandler(IReceptionistProfileRepository receptionistRepository)
    {
        _receptionistRepository = receptionistRepository;
    }

    public async Task<Guid> Handle(CreateReceptionistProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = new ReceptionistProfile
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            OwnerId = request.OwnerId,
            OfficeId = request.OfficeId
        };

        await _receptionistRepository.AddAsync(profile, cancellationToken);

        return profile.Id;
    }
}
