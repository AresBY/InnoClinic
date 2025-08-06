using System.Security.Claims;

using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreatePatientProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllPatients;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetPatientProfile;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = nameof(UserRole.Patient))]
        public async Task<IActionResult> CreatePatientProfile([FromBody] CreatePatientProfileCommand command, CancellationToken cancellationToken)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            command.OwnerId = userId;

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

        /// <summary>
        /// Returns the authenticated patient's profile.
        /// </summary>
        /// <returns>Patient profile data</returns>
        [HttpGet(nameof(GetPatientProfile))]
        [Authorize(Roles = nameof(UserRole.Patient))]
        public async Task<IActionResult> GetPatientProfile()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized("User ID is missing from the token.");
            }

            var query = new GetPatientProfileQuery(Guid.Parse(patientId));
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
