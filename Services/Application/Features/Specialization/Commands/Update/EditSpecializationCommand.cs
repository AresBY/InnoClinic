using InnoClinic.Services.Application.DTOs;

using MediatR;

namespace InnoClinic.Services.Application.Features.Specialization.Commands.Update
{
    public sealed record EditSpecializationCommand(
       Guid Id,
       string Name,
       bool IsActive,
       List<EditServiceDto> Services
   ) : IRequest<Unit>;
}
