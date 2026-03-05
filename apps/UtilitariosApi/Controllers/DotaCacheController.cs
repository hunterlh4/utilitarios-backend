using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.DotaCaches.Actions;
using UtilitariosCore.Application.Features.DotaCaches.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/dota-cache")]
public class DotaCacheController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DotaCacheDto>>> GetAll([FromQuery] int? treasureId = null)
    {
        var result = await sender.Send(new GetAllDotaCachesQuery(treasureId));
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateDotaCacheCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDotaCacheCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteDotaCacheCommand(id));
        return result.ToActionResult();
    }
}
