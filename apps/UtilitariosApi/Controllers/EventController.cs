using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Events.Actions;
using UtilitariosCore.Application.Features.Events.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/event")]
[ApiController]
public class EventController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetByMonth([FromQuery] int? year, [FromQuery] int? month)
    {
        var now = DateTime.Now;
        var response = await sender.Send(new GetEventsByMonthQuery(
            year ?? now.Year,
            month ?? now.Month
        ));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("{eventId}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] string eventId)
    {
        var response = await sender.Send(new DeleteEventCommand(eventId));
        return response.ToActionResult();
    }
}
