using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Actresses.Actions;
using UtilitariosCore.Application.Features.Actresses.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/actress-jav")]
[ApiController]
public class ActressJavController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActressJavDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllActressesQuery());
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateActressJavDto>> Create([FromBody] CreateActressCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateActressJavDto payload)
    {
        var response = await sender.Send(new UpdateActressCommand
        {
            Id = id,
            Name = payload.Name
        });
        return response.ToActionResult();
    }
}
