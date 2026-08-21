using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosCore.Application.Features.Gastos.Actions;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Shared.Extensions;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GastoController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<long>> CreateGasto([FromBody] CreateGastoDto gasto)
    {
        var response = await sender.Send(new CreateGastoCommand(gasto));
        return response.ToActionResult();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGasto([FromRoute] long id, [FromBody] UpdateGastoDto gasto)
    {
        var response = await sender.Send(new UpdateGastoCommand(id, gasto));
        return response.ToActionResult();
    }

    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<GastoDto>>> GetGastosByDateRange(
        [FromQuery] DateTime fechaInicio, 
        [FromQuery] DateTime fechaFin)
    {
        var response = await sender.Send(new GetGastosByDateRangeQuery(fechaInicio, fechaFin));
        return response.ToActionResult();
    }

    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<GastoReporteDto>>> GetGastosReport(
        [FromQuery] DateTime fechaInicio, 
        [FromQuery] DateTime fechaFin)
    {
        var response = await sender.Send(new GetGastosReportQuery(fechaInicio, fechaFin));
        return response.ToActionResult();
    }

    [HttpGet("resumen")]
    public async Task<ActionResult<ResumenGastosDto>> GetResumenGastos(
        [FromQuery] DateTime fechaInicio, 
        [FromQuery] DateTime fechaFin)
    {
        var response = await sender.Send(new GetResumenGastosQuery(fechaInicio, fechaFin));
        return response.ToActionResult();
    }
}
