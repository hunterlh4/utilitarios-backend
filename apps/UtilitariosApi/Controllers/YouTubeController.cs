using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.YouTubes.Actions;
using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;

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
