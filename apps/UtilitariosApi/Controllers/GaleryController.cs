using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.AnimeGaleries.Actions;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Application.Features.GirlGaleries.Actions;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Application.Features.Shared.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/galery")]
[ApiController]
public class GaleryController(ISender sender) : ControllerBase
{
    #region  anime-galery
    [HttpGet("anime")]
    public async Task<ActionResult<IEnumerable<AnimeGaleryDto>>> GetAllAnime()
    {
        var response = await sender.Send(new GetAllAnimeGaleriesQuery());
        return response.ToActionResult();
    }

    [HttpGet("anime/{id:int}")]
    public async Task<ActionResult<AnimeGaleryDetailDto>> GetAnimeById([FromRoute] int id)
    {
        var response = await sender.Send(new GetAnimeGaleryByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost("anime")]
    public async Task<ActionResult<CreateAnimeGaleryDto>> CreateAnime([FromBody] CreateAnimeGaleryCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("anime/{id:int}")]
    public async Task<ActionResult> UpdateAnime([FromRoute] int id, [FromBody] UpdateAnimeGaleryDto payload)
    {
        var response = await sender.Send(new UpdateAnimeGaleryCommand(id)
        {
            Name = payload.Name
        });
        return response.ToActionResult();
    }

    [HttpPut("anime/{id:int}/links")]
    public async Task<ActionResult> UpdateAnimeLinks([FromRoute] int id, [FromBody] List<string> links)
    {
        var response = await sender.Send(new UpdateAnimeGaleryLinksCommand(id, links));
        return response.ToActionResult();
    }

    [HttpDelete("anime/{id:int}")]
    public async Task<ActionResult> DeleteAnime([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteAnimeGaleryCommand(id));
        return response.ToActionResult();
    }

    [HttpGet("anime/export")]
    public async Task<ActionResult<ExcelFileDto>> ExportAnime()
    {
        var response = await sender.Send(new ExportAnimeGaleryExcelQuery());
        return response.ToActionResult();
    }

    [HttpPost("anime/import")]
    public async Task<ActionResult<ImportExcelResult>> ImportAnime([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportAnimeGaleryExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost("anime/{id:int}/image")]
    public async Task<ActionResult> UploadAnimeImage([FromRoute] int id, [FromForm] IFormFile image)
    {
        if (image is null || image.Length == 0)
            return BadRequest("La imagen es requerida.");

        await using var stream = image.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new UploadAnimeGaleryImageCommand
        {
            GaleryId = id,
            ImageData = memory.ToArray(),
            FileName = image.FileName
        });

        return response.ToActionResult();
    }
    #endregion

    #region girls-galery

    [HttpGet("girl")]
    public async Task<ActionResult<IEnumerable<GirlGaleryDto>>> GetAllGirls()
    {
        var response = await sender.Send(new GetAllGirlGaleriesQuery());
        return response.ToActionResult();
    }

    [HttpGet("girl/{id:int}")]
    public async Task<ActionResult<GirlGaleryDetailDto>> GetGirlsById([FromRoute] int id)
    {
        var response = await sender.Send(new GetGirlGaleryByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost("girl")]
    public async Task<ActionResult<CreateGirlGaleryDto>> CreateGirls([FromBody] CreateGirlGaleryCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("girl/{id:int}")]
    public async Task<ActionResult> UpdateGirls([FromRoute] int id, [FromBody] UpdateGirlGaleryDto payload)
    {
        var response = await sender.Send(new UpdateGirlGaleryCommand(id)
        {
            Name = payload.Name
        });
        return response.ToActionResult();
    }

    [HttpPut("girl/{id:int}/links")]
    public async Task<ActionResult> UpdateGirlsLinks([FromRoute] int id, [FromBody] List<string> links)
    {
        var response = await sender.Send(new UpdateGirlGaleryLinksCommand(id, links));
        return response.ToActionResult();
    }

    [HttpDelete("girl/{id:int}")]
    public async Task<ActionResult> DeleteGirls([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteGirlGaleryCommand(id));
        return response.ToActionResult();
    }

    [HttpGet("girl/export")]
    public async Task<ActionResult<ExcelFileDto>> ExportGirls()
    {
        var response = await sender.Send(new ExportGirlGaleryExcelQuery());
        return response.ToActionResult();
    }

    [HttpPost("girl/import")]
    public async Task<ActionResult<ImportExcelResult>> ImportGirls([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportGirlGaleryExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost("girl/{id:int}/image")]
    public async Task<ActionResult> UploadGirlsImage([FromRoute] int id, [FromForm] IFormFile image)
    {
        if (image is null || image.Length == 0)
            return BadRequest("La imagen es requerida.");

        await using var stream = image.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new UploadGirlGaleryImageCommand
        {
            GaleryId = id,
            ImageData = memory.ToArray(),
            FileName = image.FileName
        });

        return response.ToActionResult();
    }

    #endregion
}
