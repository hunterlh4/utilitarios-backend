using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Helpers;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Gastos.Actions;

public record UpdateGastoCommand(long Id, UpdateGastoDto Gasto) : IRequest<Result>;

public sealed class UpdateGastoCommandValidator : AbstractValidator<UpdateGastoCommand>
{
    public UpdateGastoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID inválido");

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

internal sealed class UpdateGastoCommandHandler(IGastoRepository gastoRepository)
    : IRequestHandler<UpdateGastoCommand, Result>
{
    public async Task<Result> Handle(UpdateGastoCommand request, CancellationToken cancellationToken)
    {
        var existingGasto = await gastoRepository.GetGastoById(request.Id);
        if (existingGasto == null)
            return Errors.NotFound("Gasto no encontrado");

        // Convertir el monto al signo correcto según el tipo
        var montoCorregido = TipoGastoHelper.ConvertirMontoSegunTipo(request.Gasto.Monto, request.Gasto.TipoGastoId);

        existingGasto.TipoGastoId = request.Gasto.TipoGastoId;
        existingGasto.Monto = montoCorregido;
        existingGasto.Fecha = request.Gasto.Fecha;
        existingGasto.Descripcion = request.Gasto.Descripcion;

        var updated = await gastoRepository.UpdateGasto(existingGasto);
        return updated ? Results.NoContent() : Errors.BadRequest("No se pudo actualizar el gasto");
    }
}