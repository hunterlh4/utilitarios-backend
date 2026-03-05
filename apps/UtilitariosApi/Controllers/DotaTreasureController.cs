using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.DotaTreasures.Actions;
using UtilitariosCore.Application.Features.DotaTreasures.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/dota-treasure")]
public class DotaTreasureController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DotaTreasureDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllDotaTreasuresQuery());
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateDotaTreasureCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDotaTreasureCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteDotaTreasureCommand(id));
        return result.ToActionResult();
    }
}
