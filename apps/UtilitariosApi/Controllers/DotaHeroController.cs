using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.DotaHeroes.Actions;
using UtilitariosCore.Application.Features.DotaHeroes.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/dota-hero")]
public class DotaHeroController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DotaHeroDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllDotaHeroesQuery());
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateDotaHeroCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDotaHeroCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteDotaHeroCommand(id));
        return result.ToActionResult();
    }
}
