using InnoClinic.Services.Application.Features.Service.Queries.ViewServices;
using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Application.Features.Services.Commands.EditService;
using InnoClinic.Services.Application.Features.Services.Queries.ViewService;

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

        /// <summary>
        /// Retrieves detailed information about a specific service.
        /// </summary>
        /// <param name="serviceId">The ID of the service to view.</param>
        /// <returns>Returns detailed info of the service.</returns>
        [HttpGet("{serviceId:guid}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> ViewService(Guid serviceId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ViewServiceQuery(serviceId));
            return Ok(result);
        }

        /// <summary>
        /// Changes the status of a service (Active/Inactive).
        /// </summary>
        /// <remarks>
        /// Only Receptionist can change service status. When set to Inactive, the service becomes invisible to patients.
        /// </remarks>
        /// <param name="command">Command containing ServiceId and new Status.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Returns HTTP 200 if status is successfully updated.</returns>
        [HttpPatch("ChangeStatus")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeServiceStatusCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
