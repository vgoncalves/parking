using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ParkingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetParkings([FromRoute] string id, CancellationToken cancellationToken)
    {
        var request = new GetParking.Request() { Id = id };
        
        var response = await _mediator.Send(request, cancellationToken);

        if (response.Payload == null)
            return NotFound();

        return Ok(response.Payload);
    }

    [HttpPost]
    public async Task<IActionResult> CreateParking(CreateParking.Request request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        if (response.FailureReason == FailureReason.InvalidRequest)
            return BadRequest(response.Errors);

        return CreatedAtAction(nameof(GetParkings), new { id = response.Payload!.Id }, response.Payload);
    }
}