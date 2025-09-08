using InnoClinic.Services.Application.Features.Specialization.Commands.ChangeStatus;
using InnoClinic.Services.Application.Features.Specialization.Commands.CreateSpecialization;
using InnoClinic.Services.Application.Features.Specialization.Commands.Update;
using InnoClinic.Services.Application.Features.Specialization.Queries.GetAll;
using InnoClinic.Services.Application.Features.Specialization.Queries.GetById;

using MediatR;

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

        /// <summary>
        /// Creates a new specialization.
        /// Accessible only for Receptionists.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { SpecializationId = result });
        }

        /// <summary>
        /// Changes the status of an existing specialization (Active/Inactive).
        /// Accessible only for Receptionists.
        /// </summary>
        [HttpPatch("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeSpecializationStatusCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Returns full information about a specialization by its Id,
        /// including related services.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new ViewSpecializationByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns a list of all specializations in the system.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new ViewSpecializationsListQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Updates the information of an existing specialization,
        /// including its name, status, and services.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] EditSpecializationCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
