using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Actresses.Actions;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Application.Features.Actresses.Requests;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActressJavDetailDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetActressJavByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateActressJavDto>> Create([FromBody] CreateActressCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateActressJavRequest request)
    {
        var response = await sender.Send(new UpdateActressCommand { Id = id, Name = request.Name });
        return response.ToActionResult();
    }

    [HttpPut("{id:int}/links")]
    public async Task<ActionResult> UpdateLinks([FromRoute] int id, [FromBody] List<string> links)
    {
        var response = await sender.Send(new UpdateActressJavLinksCommand(id, links));
        return response.ToActionResult();
    }
}
