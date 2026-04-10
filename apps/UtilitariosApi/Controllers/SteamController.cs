using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.DotaCaches.Actions;
using UtilitariosCore.Application.Features.DotaCaches.Dtos;
using UtilitariosCore.Application.Features.DotaHeroes.Actions;
using UtilitariosCore.Application.Features.DotaHeroes.Dtos;
using UtilitariosCore.Application.Features.DotaTreasures.Actions;
using UtilitariosCore.Application.Features.DotaTreasures.Dtos;
using UtilitariosCore.Application.Features.Steam.Actions;
using UtilitariosCore.Application.Features.SteamItemDrops.Actions;
using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;
using UtilitariosCore.Application.Features.SteamItemPurchases.Actions;
using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;
using UtilitariosCore.Application.Features.SteamItems.Actions;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/steam")]
public class SteamController(ISender sender) : ControllerBase
{
    #region Steam-item
    [HttpGet("item")]
    public async Task<ActionResult<IEnumerable<SteamItemDto>>> GetAllItems()
    {
        var result = await sender.Send(new GetAllSItemsQuery());
        return result.ToActionResult();
    }

    [HttpGet("item-api-search")]
    public async Task<ActionResult<JsonElement>> SteamApiSearch([FromQuery] string query, [FromQuery] int game = 1)
    {
        var result = await sender.Send(new SteamApiSearchQuery(query, game));
        return result.ToActionResult();
    }

    [HttpPost("item")]
    public async Task<ActionResult<int>> Create([FromBody] CreateSteamItemCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPost("item-bulk")]
    public async Task<ActionResult<BulkCreateSteamItemResult>> BulkCreate([FromBody] List<BulkCreateSteamItemDto> items)
    {
        var result = await sender.Send(new BulkCreateSteamItemCommand { Items = items });
        return result.ToActionResult();
    }

    [HttpGet("item/export")]
    public async Task<ActionResult<ExcelFileDto>> ExportItemsExcel()
    {
        var result = await sender.Send(new ExportSteamItemsExcelQuery());
        return result.ToActionResult();
    }

    [HttpPost("item/import")]
    public async Task<ActionResult<ImportExcelResult>> ImportItemsExcel([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var result = await sender.Send(new ImportSteamItemsExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return result.ToActionResult();
    }

    [HttpPut("item/{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateItemCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("item/{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var result = await sender.Send(new DeleteItemCommand(id));
        return result.ToActionResult();
    }
    #endregion

    #region Steam-drop
    [HttpGet("drop")]
    public async Task<ActionResult<IEnumerable<SteamItemDropDto>>> GetAllDrops()
    {
        var result = await sender.Send(new GetAllItemDropsQuery());
        return result.ToActionResult();
    }

    [HttpPost("drop")]
    public async Task<ActionResult<int>> CreateDrop([FromBody] CreateItemDropCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("drop/{id:int}")]
    public async Task<ActionResult> UpdateDrop([FromRoute] int id, [FromBody] UpdateItemDropCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("drop/{id:int}")]
    public async Task<ActionResult> DeleteDrop([FromRoute] int id)
    {
        var result = await sender.Send(new DeleteItemDropCommand(id));
        return result.ToActionResult();
    }
    #endregion

    #region Steam-purchase
    [HttpGet("purchase")]
    public async Task<ActionResult<IEnumerable<SteamItemPurchaseDto>>> GetAllPurchases()
    {
        var result = await sender.Send(new GetAllItemPurchasesQuery());
        return result.ToActionResult();
    }

    [HttpPost("purchase")]
    public async Task<ActionResult<int>> CreatePurchase([FromBody] CreateSteamItemPurchaseCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("purchase/{id:int}")]
    public async Task<ActionResult> UpdatePurchase([FromRoute] int id, [FromBody] UpdateItemPurchaseCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("purchase/{id:int}")]
    public async Task<ActionResult> DeletePurchase([FromRoute] int id)
    {
        var result = await sender.Send(new DeleteItemPurchaseCommand(id));
        return result.ToActionResult();
    }
    #endregion

    #region hero

    [HttpGet("hero")]
    public async Task<ActionResult<IEnumerable<DotaHeroDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllHeroesQuery());
        return result.ToActionResult();
    }

    [HttpPost("hero")]
    public async Task<ActionResult<int>> CreateHero([FromBody] CreateHeroCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("hero/{id:int}")]
    public async Task<ActionResult> UpdateHero(int id, [FromBody] UpdateHeroCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("hero/{id:int}")]
    public async Task<ActionResult> DeleteHero(int id)
    {
        var result = await sender.Send(new DeleteHeroCommand(id));
        return result.ToActionResult();
    }
    #endregion

    #region treasure
    [HttpGet("treasure")]
    public async Task<ActionResult<IEnumerable<DotaTreasureDto>>> GetAllTreasure()
    {
        var result = await sender.Send(new GetAllTreasuresQuery());
        return result.ToActionResult();
    }

    [HttpPost("treasure")]
    public async Task<ActionResult<int>> CreateTreasure([FromBody] CreateTreasureCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("treasure/{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTreasureCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("treasure/{id:int}")]
    public async Task<ActionResult> DeleteTreasure(int id)
    {
        var result = await sender.Send(new DeleteTreasureCommand(id));
        return result.ToActionResult();
    }
    #endregion

    #region cache
    [HttpGet("cache")]
    public async Task<ActionResult<IEnumerable<DotaCacheDto>>> GetAllCache()
    {
        var result = await sender.Send(new GetAllCachesQuery());
        return result.ToActionResult();
    }

    [HttpGet("cache/{id:int}")]
    public async Task<ActionResult<DotaCacheDto>> GetCacheById([FromRoute] int id)
    {
        var result = await sender.Send(new GetCacheByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpPost("cache")]
    public async Task<ActionResult<int>> CreateCache([FromBody] CreateCacheCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("cache/{id:int}")]
    public async Task<ActionResult> UpdateCache(int id, [FromBody] UpdateCacheCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("cache/{id:int}")]
    public async Task<ActionResult> DeleteCache(int id)
    {
        var result = await sender.Send(new DeleteCacheCommand(id));
        return result.ToActionResult();
    }
    #endregion
}
