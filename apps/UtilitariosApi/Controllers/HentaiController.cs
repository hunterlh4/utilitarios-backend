using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Hentais.Actions;
using UtilitariosCore.Application.Features.Hentais.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/hentai")]
[ApiController]
public class HentaiController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HentaiDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllHentaisQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HentaiDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetHentaiByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportHentaiExcelQuery());
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

        var response = await sender.Send(new ImportHentaiExcelCommand
        {
            FileBytes = memory.ToArray()
        });

        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateHentaiDto>> Create([FromBody] CreateHentaiCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}/tags")]
    public async Task<ActionResult> UpdateTags([FromRoute] int id, [FromBody] List<int> tagIds)
    {
        var response = await sender.Send(new UpdateHentaiTagsCommand(id, tagIds));
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteHentaiCommand(id));
        return response.ToActionResult();
    }
}
