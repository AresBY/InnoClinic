using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreatePatientProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllPatients;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers
{
    /// <summary>
    /// Handles operations related to patient profile management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new patient profile after the user has verified their email.
        /// If a similar profile exists (based on weighted match logic), it will be returned for confirmation.
        /// Otherwise, a new profile is created and linked to the current user's account.
        /// </summary>
        /// <param name="command">The patient profile data submitted by the user.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>Created profile ID or match info.</returns>
        [HttpPost(nameof(CreatePatientProfile))]
        public async Task<IActionResult> CreatePatientProfile([FromBody] CreatePatientProfileCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreatePatientProfile), new { id = result }, result);
        }

        /// <summary>
        /// Retrieves all patient profiles from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A list of patient profile DTOs.</returns>
        [HttpGet(nameof(GetAllPatients))]
        public async Task<ActionResult<List<PatientProfileDto>>> GetAllPatients(CancellationToken cancellationToken)
        {
            var patients = await _mediator.Send(new GetAllPatientsQuery(), cancellationToken);
            return Ok(patients);
        }
    }
}
