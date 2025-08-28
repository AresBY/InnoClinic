using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Application.Features.Services.Commands.EditService;
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

        /// <summary>
        /// Creates a new service in the system.
        /// </summary>
        /// <remarks>
        /// Only users with the role 'Receptionist' can access this endpoint.
        /// </remarks>
        /// <param name="command">The command containing the data for creating the service.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Returns the ID of the newly created service.</returns>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { ServiceId = result });
        }

        /// <summary>
        /// Retrieves the list of all services available in the system.
        /// </summary>
        /// <returns>Returns a collection of services grouped by category.</returns>
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var result = await _mediator.Send(new ViewServicesQuery());
            return Ok(result);
        }

        /// <summary>
        /// Updates the information of an existing service.
        /// </summary>
        /// <param name="command">The command containing the updated service data.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Returns HTTP 200 if the update is successful.</returns>
        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> EditService([FromBody] EditServiceCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

    }
}
