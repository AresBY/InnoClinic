using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;
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
            var office = command.ToEntity();

            office.CreatedAt = DateTimeOffset.UtcNow;

            await _repository.InsertAsync(office, cancellationToken);

            return office.Id;
        }
    }
}
