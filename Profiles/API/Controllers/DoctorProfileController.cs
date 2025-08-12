using System.Security.Claims;

using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.EditDoctorProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileByOwn;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers
{
    /// <summary>
    /// Handles operations related to doctor profile management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DoctorProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new doctor profile.
        /// </summary>
        /// <param name="command">Doctor creation data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ID of the created doctor</returns>
        [HttpPost(nameof(CreateDoctorProfile))]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreateDoctorProfile([FromBody] CreateDoctorProfileCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateDoctorProfile), new { id = result }, result);
        }

        /// <summary>
        /// Retrieves a list of all doctor profiles.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of <see cref="DoctorProfileDto"/> objects.</returns>
        [HttpGet(nameof(GetDoctorProfileAll))]
        public async Task<ActionResult<List<DoctorProfileDto>>> GetDoctorProfileAll(CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorProfileAllQuery(), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Retrieves detailed information about the specified doctor.
        /// </summary>
        /// <param name="id">The ID of the doctor.</param>
        /// <returns>Detailed information about the doctor.</returns>
        /// <response code="200">Returns the doctor's profile data</response>
        /// <response code="404">If the doctor is not found</response>
        [HttpGet(nameof(GetDoctorProfileByOwn))]
        public async Task<IActionResult> GetDoctorProfileByOwn()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(doctorId))
            {
                return Unauthorized("User ID is missing from the token.");
            }

            var query = new GetDoctorProfileByOwnQuery(Guid.Parse(doctorId));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Edits the profile of the currently authenticated Doctor or Receptionist.
        /// </summary>
        /// <param name="command">Updated doctor profile data.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated doctor profile data.</returns>
        [HttpPut(nameof(EditDoctorOrReceptionistProfileByOwn))]
        [Authorize(Roles = nameof(UserRole.Doctor) + "," + nameof(UserRole.Receptionist))]
        public async Task<IActionResult> EditDoctorOrReceptionistProfileByOwn([FromBody] EditDoctorOrReceptionistProfileByOwnCommand command, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing from the token.");
            }
            command.Id = Guid.Parse(userId);

            var updatedProfile = await _mediator.Send(command, cancellationToken);
            return Ok(updatedProfile);
        }
    }
}
