using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice;
using InnoClinicCommon.Enums;

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
        [Authorize(Roles = nameof(UserRole.Receptionist) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateOffice([FromBody] CreateOfficeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateOffice), new { id = result }, result);
        }
    }
}
