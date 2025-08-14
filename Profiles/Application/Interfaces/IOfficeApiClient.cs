using InnoClinic.Profiles.Application.DTOs;

namespace InnoClinic.Profiles.Application.Interfaces
{
    public interface IOfficeApiClient
    {
        Task<List<OfficeMapDto>> GetOfficesForMapAsync(CancellationToken cancellationToken);

        Task<OfficeAddressDto> GetOfficeAddressAsync(Guid officeId, CancellationToken cancellationToken);
    }
}
