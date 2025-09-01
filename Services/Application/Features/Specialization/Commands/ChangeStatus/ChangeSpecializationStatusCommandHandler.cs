using System.Net.Http.Json;

using InnoClinic.Services.Application.DTOs;
using InnoClinic.Services.Application.Features.Specialization.Commands.ChangeStatus;
using InnoClinic.Services.Application.Interfaces.Repositories;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Http;

public class ChangeSpecializationStatusCommandHandler : IRequestHandler<ChangeSpecializationStatusCommand, Unit>
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangeSpecializationStatusCommandHandler(
        ISpecializationRepository specializationRepository,
        IServiceRepository serviceRepository,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _specializationRepository = specializationRepository;
        _serviceRepository = serviceRepository;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(ChangeSpecializationStatusCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _specializationRepository.GetByIdAsync(request.SpecializationId, cancellationToken);
        if (specialization is null)
            throw new KeyNotFoundException($"Specialization with id {request.SpecializationId} not found");

        specialization.IsActive = request.IsActive;
        await _specializationRepository.UpdateAsync(specialization, cancellationToken);

        var services = await _serviceRepository.GetBySpecializationIdAsync(request.SpecializationId, cancellationToken);
        foreach (var service in services)
        {
            service.IsActive = request.IsActive;
            await _serviceRepository.UpdateAsync(service, cancellationToken);
        }

        if (!request.IsActive)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var userToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(userToken))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken.Replace("Bearer ", ""));
            }

            var doctorIdsResponse = await httpClient.GetFromJsonAsync<List<DoctorIdDto>>(
                $"http://localhost:5181/api/doctorprofile/filter/by-specialization?specializationId={request.SpecializationId}",
                cancellationToken);

            if (doctorIdsResponse != null)
            {
                foreach (var doctor in doctorIdsResponse)
                {
                    var response = await httpClient.PutAsJsonAsync(
                        $"http://localhost:5181/api/doctorprofile/{doctor.Id}/status",
                        DoctorStatus.Inactive,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();
                }
            }
        }

        return Unit.Value;
    }
}
