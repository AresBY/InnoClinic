using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Application.Features.Services.Queries.ViewServices;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Services.API.Controllers
{
    /// <summary>
    /// Handles authentication-related operations (registration, login, etc.)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { ServiceId = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var result = await _mediator.Send(new ViewServicesQuery());
            return Ok(result);
        }
    }
}
