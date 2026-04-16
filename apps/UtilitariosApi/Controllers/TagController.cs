using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Tags.Actions;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/tag")]
[ApiController]
public class TagController(ISender sender) : ControllerBase
{
    [HttpGet("type/{type:int}")]
    public async Task<ActionResult<IEnumerable<Tag>>> GetByType([FromRoute] int type)
    {
        var response = await sender.Send(new GetTagsByTypeQuery((TagType)type));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTagCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateTagCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteTagCommand(id));
        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportAll()
    {
        var response = await sender.Send(new ExportTagsExcelQuery());
        return response.ToActionResult();
    }

    [HttpPost("import")]
    public async Task<ActionResult<ImportExcelResult>> ImportAll([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportTagsExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }
}

