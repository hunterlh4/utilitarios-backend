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

    [HttpPut("{id:int}/tags")]
    public async Task<ActionResult> UpdateTags([FromRoute] int id, [FromBody] List<string> tags)
    {
        var response = await sender.Send(new UpdateHentaiTagsCommand(id, tags));
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteHentaiCommand(id));
        return response.ToActionResult();
    }
}
