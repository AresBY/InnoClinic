using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Commands.UpdateOffice;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, string>
{
    private readonly IOfficeRepository _repository;

    public UpdateOfficeCommandHandler(IOfficeRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (office == null)
            throw new NotFoundException($"Office with id {request.Id} not found.");

        office = request.ToEntity();

        await _repository.UpdateAsync(office, cancellationToken);

        return office.Id;
    }
}
