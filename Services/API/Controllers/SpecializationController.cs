using InnoClinic.Services.Application.Features.Specialization.Commands.CreateSpecialization;

using InnoClinicCommon.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Specializations.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SpecializationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecializationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Receptionist))]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { SpecializationId = result });
        }
    }
}
