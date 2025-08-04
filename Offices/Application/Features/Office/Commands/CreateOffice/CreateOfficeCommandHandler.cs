using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;

using MediatR;


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
            var office = command.ToEntity();

            office.CreatedAt = DateTimeOffset.UtcNow;

            await _repository.InsertAsync(office, cancellationToken);

            return office.Id;
        }
    }
}
