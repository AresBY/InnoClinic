using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinicCommon.Exception;
using InnoClinic.Offices.Application.Mappings;
using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Commands;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, string>
{
    private readonly IOfficeRepository _repository;

    public UpdateCommandHandler(IOfficeRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var office = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (office == null)
            throw new NotFoundException($"Office with id {request.Id} not found.");

        office = request.ToEntity();

        await _repository.UpdateAsync(office, cancellationToken);

        return office.Id;
    }
}
