using InnoClinic.Services.Domain.Enums;

namespace InnoClinic.Services.Application.DTOs
{
    public sealed record EditServiceDto(
       Guid? Id,
       string Name,
       decimal Price,
       ServiceCategory Category,
       bool IsActive
   );
}
