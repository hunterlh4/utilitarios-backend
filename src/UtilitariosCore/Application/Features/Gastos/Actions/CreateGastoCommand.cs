using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Helpers;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Gastos.Actions;

public record CreateGastoCommand(CreateGastoDto Gasto) : IRequest<Result<long>>;

public sealed class CreateGastoCommandValidator : AbstractValidator<CreateGastoCommand>
{
    public CreateGastoCommandValidator()
    {
        RuleFor(x => x.Gasto.TipoGastoId)
            .IsInEnum()
            .WithMessage("Tipo de gasto inválido");

        RuleFor(x => x.Gasto.Monto)
            .NotEqual(0)
            .WithMessage("El monto no puede ser cero");

        RuleFor(x => x.Gasto.Fecha)
            .NotEmpty()
            .WithMessage("La fecha es requerida");
    }
}

internal sealed class CreateGastoCommandHandler(IGastoRepository gastoRepository)
    : IRequestHandler<CreateGastoCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
    {
        // Convertir el monto al signo correcto según el tipo
        var montoCorregido = TipoGastoHelper.ConvertirMontoSegunTipo(request.Gasto.Monto, request.Gasto.TipoGastoId);

        var gasto = new Gasto
        {
            TipoGastoId = request.Gasto.TipoGastoId,
            Monto = montoCorregido,
            Fecha = request.Gasto.Fecha,
            Descripcion = request.Gasto.Descripcion,
            CreatedAt = DateTime.UtcNow
        };

        var id = await gastoRepository.CreateGasto(gasto);
        return Results.Success(id);
    }
}