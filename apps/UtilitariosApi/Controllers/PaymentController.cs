using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Payments.Actions;
using UtilitariosCore.Application.Features.Payments.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDetailDto>>> GetAll()
    {
        var response = await sender.Send(new GetAllPaymentsQuery());
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentDetailDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetPaymentByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePaymentCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPost("{id:int}/detail")]
    public async Task<ActionResult<int>> AddDetail([FromRoute] int id, [FromBody] AddPaymentDetailCommand command)
    {
        var response = await sender.Send(command with { PaymentId = id });
        return response.ToActionResult();
    }

    [HttpPut("{id:int}/detail/{detailId:int}")]
    public async Task<ActionResult<bool>> UpdateDetail([FromRoute] int id, [FromRoute] int detailId, [FromBody] UpdatePaymentDetailCommand command)
    {
        var response = await sender.Send(command with { DetailId = detailId });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeletePaymentCommand(id));
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}/detail/{detailId:int}")]
    public async Task<ActionResult> DeleteDetail([FromRoute] int id, [FromRoute] int detailId)
    {
        var response = await sender.Send(new DeletePaymentDetailCommand(detailId));
        return response.ToActionResult();
    }
}
