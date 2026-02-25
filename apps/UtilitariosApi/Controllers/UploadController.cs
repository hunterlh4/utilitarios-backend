using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Media.Actions;
using UtilitariosCore.Application.Features.Media.Dtos;
using UtilitariosCore.Domain.Enums;

namespace UtilitariosApi.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController(ISender sender) : ControllerBase
{
    [HttpPost("image")]
    public async Task<ActionResult<UploadImageDto>> UploadImage(
        [FromForm] IFormFile image,
        [FromForm] MediaType type,
        [FromForm] int refId)
    {
        if (image == null || image.Length == 0)
            return BadRequest(new { error = "La imagen es requerida." });

        using var ms = new MemoryStream();
        await image.CopyToAsync(ms);

        var response = await sender.Send(new UploadImageCommand(ms.ToArray(), image.FileName, type, refId));
        return response.ToActionResult();
    }

    [HttpDelete("media/{id:int}")]
    public async Task<ActionResult> DeleteMedia([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteMediaCommand(id));
        return response.ToActionResult();
    }
}
