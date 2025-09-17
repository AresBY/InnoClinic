using System.Security.Claims;

using InnoClinic.Offices.Domain.Enums;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.ChangeDoctorStatus;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.EditDoctorOrReceptionistProfileByOwn;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileByOwn;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorProfileForReceptionist;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsByFilter;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsBySpecialization;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.GetOfficesForMapFromApi;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByName;
using InnoClinic.Profiles.Application.Features.Doctor.Queries.SearchDoctorByNameForReceptionist;

using InnoClinicCommon.Enums;

using MediatR;

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
        /// POST /api/doctorprofile
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDoctorProfile([FromBody] CreateDoctorProfileCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetDoctorProfileByOwn), new { id = result }, result);
        }

        /// <summary>
        /// Returns a list of doctors optionally filtered by specialization.
        /// GET /api/doctorprofile
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] DoctorSpecialization? specialization, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorProfileAllQuery(specialization), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Retrieves detailed information about the currently authenticated doctor.
        /// GET /api/doctorprofile/me
        /// </summary>
        [HttpGet("me")]
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
        /// PUT /api/doctorprofile/me
        /// </summary>
        [HttpPut("me")]
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

        /// <summary>
        /// Changes the status of a doctor.
        /// Only accessible by users with the Receptionist role.
        /// </summary>
        /// <param name="doctorId">ID of the doctor whose status is to be changed.</param>
        /// <param name="status">New status to assign to the doctor.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns HTTP 204 No Content on success.</returns>
        [HttpPut("{doctorId}/status")]
        public async Task<IActionResult> ChangeDoctorStatus(Guid doctorId, [FromBody] DoctorStatus status, CancellationToken cancellationToken)
        {
            var command = new ChangeDoctorStatusCommand
            {
                DoctorId = doctorId,
                Status = status
            };

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Returns a list of doctors filtered by the specified office.
        /// </summary>
        /// <param name="officeId">
        /// The ID of the office to filter by (optional). 
        /// If not provided, all doctors are returned.
        /// </param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>HTTP 200 with a collection of doctors matching the filter.</returns>

        [HttpGet("filter")]
        public async Task<IActionResult> GetDoctorsByFilter([FromQuery] Guid? officeId, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorsByFilterQuery(officeId), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Returns a list of doctors filtered by the specified office for Receptionist role.
        /// Only accessible by users in the "Receptionist" role.
        /// </summary>
        /// <param name="officeId">
        /// Optional office ID to filter doctors. 
        /// If not provided, all doctors are returned.
        /// </param>
        /// <param name="cancellationToken">Cancellation token for request cancellation.</param>
        /// <returns>HTTP 200 with a list of DoctorProfileDto objects matching the office filter.</returns>
        [HttpGet("receptionist/filter")]
        public async Task<IActionResult> GetDoctorsByOfficeForReceptionist([FromQuery] Guid? officeId, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorsByFilterQuery(officeId), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Returns a list of offices with coordinates, photo and address for displaying on the map.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for request cancellation.</param>
        /// <returns>HTTP 200 with a list of OfficeMapDto objects.</returns>
        [HttpGet("map")]
        public async Task<IActionResult> GetOfficesForMap(CancellationToken cancellationToken)
        {
            var offices = await _mediator.Send(new GetOfficesForMapFromApiQuery(), cancellationToken);
            return Ok(offices);
        }

        /// <summary>
        /// Returns a list of doctors filtered by the specified specialization.
        /// </summary>
        /// <param name="specialization">The specialization to filter by (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 200 with a collection of doctors matching the specialization.</returns>
        [HttpGet("filter/by-specialization")]
        public async Task<IActionResult> GetDoctorsBySpecialization([FromQuery] DoctorSpecialization? specialization, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new GetDoctorsBySpecializationQuery(specialization), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Searches doctors by full or partial name (first, last, middle).
        /// </summary>
        /// <param name="name">Name to search for (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 200 with list of matching doctors.</returns>
        [HttpGet("search/by-name")]
        public async Task<IActionResult> SearchDoctorsByName([FromQuery] string name, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new SearchDoctorByNameQuery(name), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Searches doctors by full or partial name for Receptionist (first, last, middle name).
        /// </summary>
        /// <param name="name">Name to search for (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 200 with list of matching doctors.</returns>
        [HttpGet("receptionist/search/by-name")]
        public async Task<IActionResult> SearchDoctorsByNameForReceptionist([FromQuery] string name, CancellationToken cancellationToken)
        {
            var doctors = await _mediator.Send(new SearchDoctorByNameForReceptionistQuery(name), cancellationToken);
            return Ok(doctors);
        }

        /// <summary>
        /// Gets detailed profile of a doctor for Receptionist.
        /// </summary>
        /// <param name="id">Doctor ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP 200 with doctor profile, or 404 if not found.</returns>
        [HttpGet("receptionist/profile/{id}")]
        public async Task<IActionResult> GetDoctorProfileForReceptionist(Guid id, CancellationToken cancellationToken)
        {
            var profile = await _mediator.Send(new GetDoctorProfileForReceptionistQuery(id), cancellationToken);
            if (profile == null) return NotFound();
            return Ok(profile);
        }
    }
}
