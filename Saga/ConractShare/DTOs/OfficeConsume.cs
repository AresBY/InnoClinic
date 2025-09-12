namespace InnoClinic.Saga.Application.DTOs
{
    public record GetOfficeRequest(Guid OfficeId);

    public record GetOfficeResponse(
        Guid Id,
        string Address
    );
}
