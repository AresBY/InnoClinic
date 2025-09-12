using InnoClinic.Saga.Application.Features.Doctor.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Saga.API.Controllers;

[ApiController]
[Route("api/saga/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors(
        [FromQuery] string? fullName,
        [FromQuery] Guid? officeId,
        [FromQuery] int? specializationId,
        CancellationToken ct)
    {
        var doctors = await _mediator.Send(
            new GetDoctorsForReceptionistQuery
            {
                FullName = fullName,
                OfficeId = officeId,
                SpecializationId = specializationId
            },
            ct
        );

        return Ok(doctors);
    }
}
