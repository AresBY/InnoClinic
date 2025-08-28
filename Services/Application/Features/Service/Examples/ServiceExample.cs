using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Domain.Enums;

using Swashbuckle.AspNetCore.Filters;

namespace InnoClinic.Services.Application.Features.Service.Examples
{
    public class CreateServiceCommandExample : IExamplesProvider<CreateServiceCommand>
    {
        public CreateServiceCommand GetExamples()
        {
            return new CreateServiceCommand
            {
                Name = "Консультация невролога",
                Price = 60.00m,
                Category = ServiceCategory.Analyses,
                Status = true
            };
        }
    }
}
