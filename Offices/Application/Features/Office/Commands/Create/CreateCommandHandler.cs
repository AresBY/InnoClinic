using InnoClinic.Offices.Application.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;


namespace InnoClinic.Offices.Application.Features.Office.Commands
{
    public class CreateCommandHandler : IRequestHandler<CreateCommand, string>
    {
        private readonly IOfficeRepository _repository;

        public CreateCommandHandler(IOfficeRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateCommand command, CancellationToken cancellationToken)
        {
            var office = new Domain.Entities.Office
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PhotoUrl = command.PhotoUrl,
                City = command.City,
                Street = command.Street,
                HouseNumber = command.HouseNumber,
                OfficeNumber = command.OfficeNumber,
                RegistryPhoneNumber = command.RegistryPhoneNumber,
                Status = command.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.InsertAsync(office, cancellationToken);
            return office.Id;
        }
    }
}
