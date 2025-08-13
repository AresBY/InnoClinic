using System.Net.Http.Json;

using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces;

public class OfficeApiClient : IOfficeApiClient
{
    private readonly HttpClient _httpClient;

    public OfficeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OfficeMapDto>> GetOfficesForMapAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<List<OfficeMapDto>>("api/office/map", cancellationToken);
        return response ?? new List<OfficeMapDto>();
    }
}
