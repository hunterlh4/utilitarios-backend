using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Gastos.Actions;

public record GetResumenGastosQuery(DateTime FechaInicio, DateTime FechaFin) : IRequest<Result<ResumenGastosDto>>;

public sealed class GetResumenGastosQueryValidator : AbstractValidator<GetResumenGastosQuery>
{
    public GetResumenGastosQueryValidator()
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

internal sealed class GetResumenGastosQueryHandler(IGastoRepository gastoRepository)
    : IRequestHandler<GetResumenGastosQuery, Result<ResumenGastosDto>>
{
    public async Task<Result<ResumenGastosDto>> Handle(GetResumenGastosQuery request, CancellationToken cancellationToken)
    {
        var resumen = await gastoRepository.GetResumenGastos(request.FechaInicio, request.FechaFin);
        return Results.Success(resumen);
    }
}