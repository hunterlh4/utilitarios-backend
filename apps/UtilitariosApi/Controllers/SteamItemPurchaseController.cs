using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.SteamItemPurchases.Actions;
using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/steam-purchase")]
public class SteamItemPurchaseController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SteamItemPurchaseDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllSteamItemPurchasesQuery());
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSteamItemPurchaseCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateSteamItemPurchaseCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteSteamItemPurchaseCommand(id));
        return result.ToActionResult();
    }
}
