using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.ActressAdults.Actions;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Application.Features.ActressAdults.Requests;

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
    public async Task<ActionResult<ActressAdultBasicDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetActressAdultBasicByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpGet("{id:int}/detail")]
    public async Task<ActionResult<ActressAdultDetailDto>> GetDetailById([FromRoute] int id)
    {
        var response = await sender.Send(new GetActressAdultDetailByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateActressAdultDto>> Create([FromBody] CreateActressAdultCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateActressAdultRequest request)
    {
        var response = await sender.Send(new UpdateActressAdultCommand { Id = id, Name = request.Name, Tags = request.Tags });
        return response.ToActionResult();
    }

    [HttpPut("{id:int}/links")]
    public async Task<ActionResult> UpdateLinks([FromRoute] int id, [FromBody] List<string> links)
    {
        var response = await sender.Send(new UpdateActressAdultLinksCommand(id, links));
        return response.ToActionResult();
    }

    [HttpPost("video")]
    public async Task<ActionResult<CreateVideoAdultDto>> CreateVideo([FromBody] CreateVideoAdultCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("video/{videoId:int}")]
    public async Task<ActionResult> UpdateVideo([FromRoute] int videoId, [FromBody] UpdateVideoAdultCommand command)
    {
        var response = await sender.Send(command with { Id = videoId });
        return response.ToActionResult();
    }

    [HttpDelete("video/{videoId:int}")]
    public async Task<ActionResult> DeleteVideo([FromRoute] int videoId)
    {
        var response = await sender.Send(new DeleteVideoAdultCommand(videoId));
        return response.ToActionResult();
    }

    [HttpPatch("video/{videoId:int}/status")]
    public async Task<ActionResult> UpdateVideoStatus([FromRoute] int videoId, [FromBody] UpdateVideoAdultStatusRequest request)
    {
        var response = await sender.Send(new UpdateVideoAdultStatusCommand { VideoId = videoId, Status = request.Status });
        return response.ToActionResult();
    }
}
