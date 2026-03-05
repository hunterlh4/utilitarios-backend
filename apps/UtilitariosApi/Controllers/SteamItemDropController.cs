using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.SteamItemDrops.Actions;
using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/steam-drop")]
public class SteamItemDropController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SteamItemDropDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllSteamItemDropsQuery());
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSteamItemDropCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateSteamItemDropCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteSteamItemDropCommand(id));
        return result.ToActionResult();
    }
}
