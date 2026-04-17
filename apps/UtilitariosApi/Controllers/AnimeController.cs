using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Animes.Actions;
using UtilitariosCore.Application.Features.Animes.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/anime")]
[ApiController]
public class AnimeController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimeDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllAnimesQuery());
        return response.ToActionResult();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateAnimeStatusCommand request)
    {
        var response = await sender.Send(request with { Id = id });
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimeDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetAnimeByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportAnimeExcelQuery());
        return response.ToActionResult();
    }

    [HttpPost("import")]
    public async Task<ActionResult<ImportExcelResult>> ImportExcel([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportAnimeExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateAnimeDto>> Create([FromBody] CreateAnimeCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteAnimeCommand(id));
        return response.ToActionResult();
    }
}
