using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.SteamItems.Actions;
using UtilitariosCore.Application.Features.SteamItems.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/steam-item")]
public class SteamItemController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SteamItemDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllSteamItemsQuery());
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSteamItemCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateSteamItemCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteSteamItemCommand(id));
        return result.ToActionResult();
    }
}
