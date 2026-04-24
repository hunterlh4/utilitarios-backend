using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Comics.Actions;
using UtilitariosCore.Application.Features.Comics.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/comic")]
[ApiController]
public class ComicController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComicDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllComicsQuery());
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateComicCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateComicCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteComicCommand(id));
        return response.ToActionResult();
    }

    [HttpPost("{id:int}/image")]
    public async Task<ActionResult> UploadImage([FromRoute] int id, [FromForm] IFormFile image)
    {
        if (image is null || image.Length == 0)
            return BadRequest("La imagen es requerida.");

        await using var stream = image.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new UploadComicImageCommand
        {
            ComicId = id,
            ImageData = memory.ToArray(),
            FileName = image.FileName
        });

        return response.ToActionResult();
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportComicExcelQuery());
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

        var response = await sender.Send(new ImportComicExcelCommand { FileBytes = memory.ToArray() });
        return response.ToActionResult();
    }
}
