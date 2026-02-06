using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Metadata;

namespace UtilitariosApi.Controllers;

[Route("api/metadata")]
[ApiController]
public class MetadataController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<MetadataResponse>> GetMetadata([FromQuery] string url)
    {
        var response = await sender.Send(new GetMetadataQuery(url));
        return response.ToActionResult();
    }
}
