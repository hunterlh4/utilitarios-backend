using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Javs.Actions;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/jav")]
[ApiController]
public class JavController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JavDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllJavsQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JavDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetJavByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("check/{code}")]
    public async Task<bool> CheckCodeExists([FromRoute] string code)
    {
        var response = await sender.Send(new CheckJavCodeExistsQuery(code));
        return response.Value;
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExcelFileDto>> ExportExcel()
    {
        var response = await sender.Send(new ExportJavExcelQuery());
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateJavDto>> Create([FromBody] CreateJavCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<CreateJavDto>> BulkCreate([FromBody] BulkCreateJavCommand command)
    {
       var response = await sender.Send(command);
       return response.ToActionResult();
    }

    [HttpPost("import")]
    public async Task<ActionResult<ImportJavExcelResult>> ImportExcel([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportJavExcelStandardCommand { FileBytes = memory.ToArray() });
        return response.ToActionResult();
    }

    [HttpPost("import-temporal")]
    public async Task<ActionResult<ImportJavExcelResult>> ImportExcelTemporal([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Archivo Excel requerido.");

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var response = await sender.Send(new ImportJavExcelTemporalCommand { FileBytes = memory.ToArray() });
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

        var response = await sender.Send(new UploadJavImageCommand
        {
            JavId = id,
            ImageData = memory.ToArray(),
            FileName = image.FileName
        });
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateJavDto payload)
    {
        var response = await sender.Send(new UpdateJavCommand(id)
        {
            Code = payload.Code,
            ActressIds = payload.ActressIds,
            TagIds = payload.TagIds,
            Image = payload.Image,
            Links = payload.Links
        });
        return response.ToActionResult();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateJavStatusCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteJavCommand(id));
        return response.ToActionResult();
    }

    
}
