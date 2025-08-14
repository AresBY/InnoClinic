using System.Security.Claims;

using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile;
using InnoClinic.Profiles.Application.Features.Patient.Commands.DeletePatientProfile;
using InnoClinic.Profiles.Application.Features.Patient.Commands.UpdatePatientProfile;
using InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByDoctor;
using InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientProfileByOwn;
using InnoClinic.Profiles.Application.Features.Patient.Queries.GetPatientsProfilesAll;

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
    public class PatientProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientProfileController(IMediator mediator)
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
        [HttpPost(nameof(CreatePatientProfileByOwn))]
        [Authorize(Roles = nameof(UserRole.Patient))]
        public async Task<IActionResult> CreatePatientProfileByOwn([FromBody] CreatePatientProfileByOwnCommand command, CancellationToken cancellationToken)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }

            command.OwnerId = userId;

            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreatePatientProfileByOwn), new { id = result }, result);
        }

        /// <summary>
        /// Retrieves all patient profiles from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A list of patient profile DTOs.</returns>
        [HttpGet(nameof(GetPatientsProfilesAll))]
        public async Task<ActionResult<List<PatientProfileDto>>> GetPatientsProfilesAll(CancellationToken cancellationToken)
        {
            var patients = await _mediator.Send(new GetPatientsProfilesAllQuery(), cancellationToken);
            return Ok(patients);
        }

        /// <summary>
        /// Returns the authenticated patient's profile.
        /// </summary>
        /// <returns>Patient profile data</returns>
        [HttpGet(nameof(GetPatientProfileByOwn))]
        [Authorize(Roles = nameof(UserRole.Patient))]
        public async Task<IActionResult> GetPatientProfileByOwn()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized("User ID is missing from the token.");
            }

            var query = new GetPatientProfileByOwnQuery(Guid.Parse(patientId));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Returns the profile of the specified patient (viewed by a doctor).
        /// </summary>
        /// <param name="patientId">The ID of the patient whose profile is being requested</param>
        /// <returns>Patient profile data</returns>
        [HttpGet(nameof(GetPatientProfileByDoctor) + "/{patientId:guid}")]
        [Authorize(Roles = nameof(UserRole.Doctor))]
        public async Task<IActionResult> GetPatientProfileByDoctor(Guid patientId)
        {
            var query = new GetPatientProfileByDoctorQuery(patientId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new patient profile by a Receptionist.
        /// </summary>
        /// <param name="request">Patient creation data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 201 with the ID of the created patient.</returns>
        [HttpPost("receptionist/create")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreatePatientProfileByReceptionist([FromBody] CreatePatientProfileByOwnCommand request, CancellationToken cancellationToken)
        {
            var patientId = await _mediator.Send(request, cancellationToken);
            return CreatedAtAction(nameof(CreatePatientProfileByOwn), new { id = patientId }, patientId);
        }

        /// <summary>
        /// Deletes a patient profile by a Receptionist.
        /// </summary>
        /// <param name="patientId">ID of the patient to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 204 No Content if deletion is successful.</returns>
        [HttpDelete("receptionist/patients/{patientId}")]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> DeletePatientProfile(Guid patientId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePatientProfileCommand(patientId), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Updates patient profile by Patient or Receptionist.
        /// </summary>
        /// <param name="request">Patient update data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 204 No Content.</returns>
        [HttpPut("edit")]
        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Receptionist)}")]
        public async Task<IActionResult> UpdatePatientProfile([FromBody] UpdatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
