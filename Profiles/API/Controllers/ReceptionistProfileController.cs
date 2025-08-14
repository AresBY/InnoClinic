using InnoClinic.Profiles.Application.Features.Receptionist.Commands.CreateReceptionistProfile;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
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
            [Authorize(Roles = nameof(UserRole.Receptionist))]
            public async Task<IActionResult> CreateReceptionistProfile([FromBody] CreateReceptionistProfileCommand request, CancellationToken cancellationToken)
            {
                var id = await _mediator.Send(request, cancellationToken);
                return CreatedAtAction(nameof(CreateReceptionistProfile), new { id }, id);
            }
        }
    }
}
