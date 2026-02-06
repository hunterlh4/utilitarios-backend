using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Animes.Actions;
using UtilitariosCore.Application.Features.Animes.Dtos;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimeDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetAnimeByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateAnimeDto>> Create([FromBody] CreateAnimeCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAnimeDto payload)
    {
        var response = await sender.Send(new UpdateAnimeCommand(id)
        {
            Title = payload.Title,
            Image = payload.Image,
            Episodes = payload.Episodes,
            Status = payload.Status
        });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteAnimeCommand(id));
        return response.ToActionResult();
    }
}
