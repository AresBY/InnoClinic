using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
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
        [HttpGet(nameof(GetDoctorsAll))]
        public async Task<ActionResult<List<DoctorProfileDto>>> GetDoctorsAll(CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorsAllQuery(), cancellationToken);
            return Ok(doctors);
        }
    }
}
