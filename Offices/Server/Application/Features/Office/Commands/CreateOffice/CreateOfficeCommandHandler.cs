using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Domain.Entities;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;


namespace InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice
{
    public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, string>
    {
        private readonly IOfficeRepository _repository;

        public CreateOfficeCommandHandler(IOfficeRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateOfficeCommand command, CancellationToken cancellationToken)
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
