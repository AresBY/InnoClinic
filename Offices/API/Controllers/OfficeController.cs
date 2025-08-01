using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InnoClinicCommon.Enums;
using InnoClinic.Offices.Application.Features.Office.Commands;
using InnoClinic.Offices.Application.Features.Office.Queries;

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
        [HttpPost(nameof(Create))]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Create), new { id = result }, result);
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
        public async Task<IActionResult> UpdateOffice(string id, [FromBody] UpdateCommand command, CancellationToken cancellationToken)
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
        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets detailed information about an office by its ID.
        /// </summary>
        /// <param name="id">Office identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Office details</returns>
        [HttpGet("Get/{id}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var office = await _mediator.Send(new GetByIdQuery(id), cancellationToken);
            if (office == null)
                return NotFound();
            return Ok(office);
        }
    }
}
