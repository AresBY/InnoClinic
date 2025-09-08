using InnoClinic.Profiles.Application.Features.Receptionist.Commands.CreateReceptionistProfile;
using InnoClinic.Profiles.Application.Features.Receptionist.Commands.DeleteReceptionistProfile;
using InnoClinic.Profiles.Application.Features.Receptionist.Commands.EditReceptionistProfile;
using InnoClinic.Profiles.Application.Features.Receptionist.Queries;
using InnoClinic.Profiles.Application.Features.Receptionist.Queries.GetReceptionistById;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers
{
    namespace InnoClinic.Profiles.API.Controllers
    {
        /// <summary>
        /// Handles operations related to patient profile management.
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]

        public class ReceptionistProfileController : ControllerBase
        {

            private readonly IMediator _mediator;

            public ReceptionistProfileController(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Creates a new receptionist profile for an existing account.
            /// </summary>
            /// <param name="request">Receptionist profile creation data including OwnerId and OfficeId.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>HTTP 201 with ID of the created receptionist profile.</returns>
            [HttpPost("receptionist/create")]
            public async Task<IActionResult> CreateReceptionistProfile([FromBody] CreateReceptionistProfileCommand request, CancellationToken cancellationToken)
            {
                var id = await _mediator.Send(request, cancellationToken);
                return CreatedAtAction(nameof(CreateReceptionistProfile), new { id }, id);
            }

            /// <summary>
            /// Deletes a receptionist profile by its ID.
            /// </summary>
            /// <param name="profileId">The ID of the receptionist profile to delete.</param>
            /// <param name="cancellationToken">Cancellation token for request cancellation.</param>
            /// <remarks>
            /// Only users with the "Receptionist" role can perform this action.
            /// Sends a MediatR command to handle the deletion logic, including removing
            /// the profile, associated account, and photo if applicable.
            /// Returns HTTP 204 No Content on successful deletion.
            /// </remarks>
            [HttpDelete("{profileId}")]
            public async Task<IActionResult> DeleteReceptionistProfile(Guid profileId, CancellationToken cancellationToken)
            {
                await _mediator.Send(new DeleteReceptionistProfileCommand { ProfileId = profileId }, cancellationToken);
                return NoContent();
            }

            /// <summary>
            /// Retrieves a list of all receptionist profiles in the clinic.
            /// Only accessible by users with the Receptionist role.
            /// Returns each receptionist's full name and associated office address.
            /// </summary>
            /// <param name="cancellationToken">Token to cancel the request if needed.</param>
            /// <returns>HTTP 200 with a list of receptionists.</returns>
            [HttpGet]
            public async Task<IActionResult> GetAllReceptionists(CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(new GetAllReceptionistsQuery(), cancellationToken);
                return Ok(result);
            }

            /// <summary>
            /// Retrieves a single receptionist profile by ID.
            /// </summary>
            /// <param name="id">Receptionist profile ID.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Receptionist profile.</returns>
            [HttpGet("{id}")]
            public async Task<IActionResult> GetReceptionistById(Guid id, CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(new GetReceptionistByIdQuery { ProfileId = id }, cancellationToken);
                return Ok(result);
            }

            /// <summary>
            /// Updates an existing receptionist's profile.
            /// </summary>
            /// <param name="request">Updated profile data, including ProfileId.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>No content if update succeeded.</returns>
            [HttpPut("edit")]
            public async Task<IActionResult> EditReceptionistProfile([FromBody] EditReceptionistProfileCommand request, CancellationToken cancellationToken)
            {
                await _mediator.Send(request, cancellationToken);
                return NoContent();
            }
        }
    }
}
