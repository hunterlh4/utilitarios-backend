using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Gastos.Actions;

public record GetGastosReportQuery(DateTime FechaInicio, DateTime FechaFin) : IRequest<Result<IEnumerable<GastoReporteDto>>>;

public sealed class GetGastosReportQueryValidator : AbstractValidator<GetGastosReportQuery>
{
    public GetGastosReportQueryValidator()
    {
        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage("Fecha de inicio es requerida");

        RuleFor(x => x.FechaFin)
            .NotEmpty()
            .WithMessage("Fecha de fin es requerida")
            .GreaterThanOrEqualTo(x => x.FechaInicio)
            .WithMessage("Fecha de fin debe ser mayor o igual a fecha de inicio");
    }
}

internal sealed class GetGastosReportQueryHandler(IGastoRepository gastoRepository)
    : IRequestHandler<GetGastosReportQuery, Result<IEnumerable<GastoReporteDto>>>
{
    public async Task<Result<IEnumerable<GastoReporteDto>>> Handle(GetGastosReportQuery request, CancellationToken cancellationToken)
    {
        var reporte = await gastoRepository.GetGastosReportByDateRange(request.FechaInicio, request.FechaFin);
        return reporte.ToList();
    }
}