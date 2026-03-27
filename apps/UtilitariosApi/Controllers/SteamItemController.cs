using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.SteamItems.Actions;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Domain.Enums;

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

    [HttpGet("steam-api-search")]
    public async Task<ActionResult<JsonElement>> SteamApiSearch([FromQuery] string query, [FromQuery] int game = 1)
    {
        var result = await sender.Send(new SteamApiSearchQuery(query, game));
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSteamItemCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<BulkCreateSteamItemResult>> BulkCreate([FromBody] List<BulkCreateSteamItemDto> items)
    {
        var result = await sender.Send(new BulkCreateSteamItemCommand(items));
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSteamItemDto dto)
    {
        var result = await sender.Send(new UpdateSteamItemCommand(id)
        {
            ExternalId = dto.ExternalId,
            Name = dto.Name,
            Image = dto.Image,
            Price = dto.Price,
            Game = dto.Game,
            MarketUrl = dto.MarketUrl,
            Status = dto.Status,
        });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var result = await sender.Send(new DeleteSteamItemCommand(id));
        return result.ToActionResult();
    }
}
