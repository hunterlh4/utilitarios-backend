using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.GirlGaleries.Actions;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Application.Features.Shared.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/girl-galery")]
[ApiController]
public class GirlGaleryController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GirlGaleryDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllGirlGaleriesQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GirlGaleryDetailDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetGirlGaleryByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateGirlGaleryDto>> Create([FromBody] CreateGirlGaleryCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateGirlGaleryDto payload)
    {
        var response = await sender.Send(new UpdateGirlGaleryCommand(id)
        {
            Name = payload.Name,
            MediaId = payload.MediaId
        });
        return response.ToActionResult();
    }

    [HttpPatch("{id:int}/media/reorder")]
    public async Task<ActionResult> ReorderMedia([FromRoute] int id, [FromBody] ReorderMediaRequest request)
    {
        var response = await sender.Send(new ReorderMediaCommand(id, request.Items));
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteGirlGaleryCommand(id));
        return response.ToActionResult();
    }
}
