using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Javs.Actions;
using UtilitariosCore.Application.Features.Javs.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/jav")]
[ApiController]
public class JavController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JavDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllJavsQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JavDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetJavByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("check/{code}")]
    public async Task<bool> CheckCodeExists([FromRoute] string code)
    {
        var response = await sender.Send(new CheckJavCodeExistsQuery(code));
        return response.Value;
    }

    [HttpPost]
    public async Task<ActionResult<CreateJavDto>> Create([FromBody] CreateJavCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    //[HttpPost("bulk")]
    //public async Task<ActionResult<CreateJavDto>> BulkCreate([FromBody] BulkCreateJavCommand command)
    //{
    //    var response = await sender.Send(command);
    //    return response.ToActionResult();
    //}

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateJavDto payload)
    {
        var response = await sender.Send(new UpdateJavCommand(id)
        {
            Code = payload.Code,
            ActressName = payload.ActressName,
            ActressUrl = payload.ActressUrl,
            Image = payload.Image,
            Links = payload.Links
        });
        return response.ToActionResult();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateJavStatusCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteJavCommand(id));
        return response.ToActionResult();
    }
}
