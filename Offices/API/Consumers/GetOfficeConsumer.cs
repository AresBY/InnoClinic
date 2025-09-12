using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Saga.Application.DTOs;

using MassTransit;

namespace InnoClinic.Offices.API.Consumers
{
    public class GetOfficeConsumer : IConsumer<GetOfficeRequest>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficeConsumer(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task Consume(ConsumeContext<GetOfficeRequest> context)
        {
            var office = await _officeRepository.GetByIdAsync(context.Message.OfficeId.ToString(), context.CancellationToken);

            await context.RespondAsync(new GetOfficeResponse(
                office != null ? Guid.Parse(office.Id) : Guid.Empty,
                office?.Address ?? string.Empty
            ));
        }
    }
}
