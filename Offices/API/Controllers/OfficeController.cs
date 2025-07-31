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
    }
}
