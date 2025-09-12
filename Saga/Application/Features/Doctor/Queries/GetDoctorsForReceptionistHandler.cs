using InnoClinic.Saga.Application.DTOs;
using InnoClinic.Saga.Application.Features.Doctor.Queries;
using InnoClinic.Saga.Contract;

using MassTransit;

using MediatR;

public class GetDoctorsForReceptionistHandler
    : IRequestHandler<GetDoctorsForReceptionistQuery, List<DoctorDto>>
{
    private readonly IRequestClient<GetDoctorsForReceptionistRequest> _doctorClient;
    private readonly IRequestClient<GetOfficeRequest> _officeClient;

    public GetDoctorsForReceptionistHandler(
        IRequestClient<GetDoctorsForReceptionistRequest> doctorClient,
        IRequestClient<GetOfficeRequest> officeClient)
    {
        _doctorClient = doctorClient;
        _officeClient = officeClient;
    }

    public async Task<List<DoctorDto>> Handle(GetDoctorsForReceptionistQuery request, CancellationToken ct)
    {
        var doctorResponse = await _doctorClient.GetResponse<GetDoctorsForReceptionistResponse>(
            new GetDoctorsForReceptionistRequest(request.FullName, request.OfficeId, request.SpecializationId),
            ct
        );

        var doctors = doctorResponse.Message.Doctors;

        var result = new List<DoctorDto>();

        foreach (var doctor in doctors)
        {
            string officeAddress = string.Empty;

            if (doctor.OfficeId != Guid.Empty)
            {
                var officeResponse = await _officeClient.GetResponse<GetOfficeResponse>(
                    new GetOfficeRequest(doctor.OfficeId),
                    ct
                );

                officeAddress = officeResponse.Message?.Address ?? string.Empty;
            }

            result.Add(new DoctorDto(
                doctor.Id,
                doctor.FullName,
                doctor.Specialization,
                doctor.Status,
                doctor.DateOfBirth,
                doctor.OfficeId,
                officeAddress
            ));
        }

        return result;
    }
}
