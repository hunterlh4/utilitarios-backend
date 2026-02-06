using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Hentais.Actions;
using UtilitariosCore.Application.Features.Hentais.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/hentai")]
[ApiController]
public class HentaiController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HentaiDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllHentaisQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HentaiDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetHentaiByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateHentaiDto>> Create([FromBody] CreateHentaiCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateHentaiDto payload)
    {
        var response = await sender.Send(new UpdateHentaiCommand(id)
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
        var response = await sender.Send(new DeleteHentaiCommand(id));
        return response.ToActionResult();
    }
}
