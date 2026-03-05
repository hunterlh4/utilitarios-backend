using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Proyects.Actions;
using UtilitariosCore.Application.Features.Proyects.Dtos;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/proyect")]
public class ProyectController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProyectDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllProyectsQuery());
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProyectDetailDto>> GetById(int id)
    {
        var result = await sender.Send(new GetProyectByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProyectCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProyectCommand command)
    {
        var result = await sender.Send(command with { Id = id });
        return result.ToActionResult();
    }

    [HttpPut("{id:int}/links")]
    public async Task<ActionResult> UpdateLinks(int id, [FromBody] UpdateProyectLinksCommand command)
    {
        var result = await sender.Send(command with { ProyectId = id });
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteProyectCommand(id));
        return result.ToActionResult();
    }
}
