using InnoClinic.Services.Domain.Enums;

namespace InnoClinic.Services.Application.DTOs
{
    public sealed record ViewServiceResultDto(
       Guid ServiceId,
       string Name,
       decimal Price,
       ServiceCategory Category,
       bool Status
   );
}
