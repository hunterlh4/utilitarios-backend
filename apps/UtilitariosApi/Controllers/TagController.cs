using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Tags.Actions;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosApi.Controllers;

[Route("api/tags")]
[ApiController]
public class TagController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetByType([FromQuery] TagType type)
    {
        var response = await sender.Send(new GetTagsByTypeQuery(type));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTagCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateTagCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteTagCommand(id));
        return response.ToActionResult();
    }
}

