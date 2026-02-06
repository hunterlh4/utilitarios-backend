using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.AnimeGaleries.Actions;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Application.Features.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/anime-galery")]
[ApiController]
public class AnimeGaleryController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimeGaleryDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllAnimeGaleriesQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimeGaleryDetailDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetAnimeGaleryByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateAnimeGaleryDto>> Create([FromBody] CreateAnimeGaleryCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAnimeGaleryDto payload)
    {
        var response = await sender.Send(new UpdateAnimeGaleryCommand(id)
        {
            Name = payload.Name
        });
        return response.ToActionResult();
    }

    [HttpPatch("{id:int}/media/reorder")]
    public async Task<ActionResult> ReorderMedia([FromRoute] int id, [FromBody] ReorderMediaRequest request)
    {
        var response = await sender.Send(new ReorderMediaCommand(id, request.Items));
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteAnimeGaleryCommand(id));
        return response.ToActionResult();
    }
}
