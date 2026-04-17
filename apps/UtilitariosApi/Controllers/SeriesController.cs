using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Series.Actions;
using UtilitariosCore.Application.Features.Series.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/series")]
[ApiController]
public class SeriesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeriesDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllSeriesQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeriesDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetSeriesByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportSeriesExcelQuery());
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

        var response = await sender.Send(new ImportSeriesExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateSeriesDto>> Create([FromBody] CreateSeriesCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

 
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateSeriesStatusCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteSeriesCommand(id));
        return response.ToActionResult();
    }
}
