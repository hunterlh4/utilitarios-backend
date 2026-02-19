using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.ActressAdults.Actions;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/actress-adult")]
[ApiController]
public class ActressAdultController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActressAdultDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllActressAdultsQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActressAdultDetailDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetActressAdultByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateActressAdultDto>> Create([FromBody] CreateActressAdultCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateActressAdultDto payload)
    {
        var response = await sender.Send(new UpdateActressAdultCommand
        {
            Id = id,
            Name = payload.Name
        });
        return response.ToActionResult();
    }

    [HttpPost("video")]
    public async Task<ActionResult<CreateVideoAdultDto>> CreateVideo([FromBody] CreateVideoAdultCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPatch("video/{videoId:int}/status")]
    public async Task<ActionResult> UpdateVideoStatus([FromRoute] int videoId, [FromBody] UpdateVideoStatusRequest request)
    {
        var response = await sender.Send(new UpdateVideoAdultStatusCommand(videoId, request.Status));
        return response.ToActionResult();
    }
}

public class UpdateVideoStatusRequest
{
    public char Status { get; set; }
}
