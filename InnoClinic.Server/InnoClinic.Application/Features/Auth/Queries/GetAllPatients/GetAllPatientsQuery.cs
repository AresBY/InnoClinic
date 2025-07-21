using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Queries;

public class GetAllPatientsQuery : IRequest<List<PatientDto>>
{
}
