using App.API.Features;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingsController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateParking.Response))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> CreateParking([FromBody] CreateParking.Request request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            if (response.IsSuccess)
                return Created("[controller]/{id}", response.Payload);

            return BadRequest(response.Errors);
        }
    }
}