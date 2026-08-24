using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Gastos.Actions;

public record GetGastosByDateRangeQuery(DateTime FechaInicio, DateTime FechaFin) : IRequest<Result<IEnumerable<GastoDto>>>;

public sealed class GetGastosByDateRangeQueryValidator : AbstractValidator<GetGastosByDateRangeQuery>
{
    public GetGastosByDateRangeQueryValidator()
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

internal sealed class GetGastosByDateRangeQueryHandler(IGastoRepository gastoRepository)
    : IRequestHandler<GetGastosByDateRangeQuery, Result<IEnumerable<GastoDto>>>
{
    public async Task<Result<IEnumerable<GastoDto>>> Handle(GetGastosByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var gastos = await gastoRepository.GetGastosByDateRange(request.FechaInicio, request.FechaFin);
        return gastos.ToList();
    }
}