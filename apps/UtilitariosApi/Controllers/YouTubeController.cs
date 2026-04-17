using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.YouTubes.Actions;
using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/youtube")]
[ApiController]
public class YouTubeController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<YouTubeDto>>> GetAll([FromQuery] YouTubeCategory? category)
    {
        var response = await sender.Send(new GetAllYouTubesQuery(category));
        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportYouTubeExcelQuery());
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

        var response = await sender.Send(new ImportYouTubeExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateYouTubeCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteYouTubeCommand(id));
        return response.ToActionResult();
    }
}
