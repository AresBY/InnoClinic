using InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice;
using InnoClinic.Offices.Application.Features.Office.Commands.UpdateOffice;
using InnoClinic.Offices.Application.Features.Office.Queries.GetOffice;
using InnoClinic.Offices.Application.Features.Office.Queries.GetOfficeAll;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Offices.API.Controllers
{
    /// <summary>
    /// Handles operations related to office management (creation, update, etc.)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new office for the clinic.
        /// </summary>
        /// <param name="command">Office creation data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ID of the created office</returns>
        [HttpPost(nameof(CreateOffice))]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreateOffice([FromBody] CreateOfficeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateOffice), new { id = result }, result);
        }

        /// <summary>
        /// Updates an existing office with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the office to update, passed in the URL.</param>
        /// <param name="command">The update command containing office details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns 200 OK if update is successful; 400 Bad Request if IDs do not match.</returns>
        [HttpPut(nameof(UpdateOffice) + "/{id}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> UpdateOffice(string id, [FromBody] UpdateOfficeCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("URL id and command id do not match.");
            }

            var result = await _mediator.Send(command, cancellationToken);

            // TODO: If the office status was changed to "Inactive",
            // then mark all related doctors and receptionists as "Inactive" as well.
            // This logic will be implemented in a future story.

            return Ok(result);
        }


        /// <summary>
        /// Gets a list of all offices.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of offices</returns>
        [HttpGet(nameof(GetOfficeAll))]
        public async Task<IActionResult> GetOfficeAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOfficeAllQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets detailed information about an office by its ID.
        /// </summary>
        /// <param name="id">Office identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Office details</returns>
        [HttpGet(nameof(GetOffice) + "/{id}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> GetOffice(string id, CancellationToken cancellationToken)
        {
            var office = await _mediator.Send(new GetOfficeByIdQuery(id), cancellationToken);
            if (office == null)
                return NotFound();
            return Ok(office);
        }
    }
}
