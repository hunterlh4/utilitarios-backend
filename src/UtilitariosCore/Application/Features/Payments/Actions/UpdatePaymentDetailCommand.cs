using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record UpdatePaymentDetailCommand : IRequest<Result<bool>>
{
    public int DetailId { get; init; }
    public int Type { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string? Description { get; init; }
}

internal sealed class UpdatePaymentDetailCommandValidator : AbstractValidator<UpdatePaymentDetailCommand>
{
    public UpdatePaymentDetailCommandValidator()
    {
        RuleFor(x => x.Type).InclusiveBetween(1, 4).WithMessage("Type debe estar entre 1 y 4.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount debe ser mayor a 0.");
        RuleFor(x => x.Date).NotEmpty().WithMessage("La fecha es requerida.");
    }
}

internal sealed class UpdatePaymentDetailCommandHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<UpdatePaymentDetailCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdatePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        var existing = await paymentRepository.GetDetailById(request.DetailId);
        if (existing is null)
            return Errors.BadRequest("El detalle de pago no existe.");

        existing.Type = request.Type;
        existing.Amount = request.Amount;
        existing.Date = request.Date.Date;
        existing.Description = request.Description;

        var success = await paymentRepository.UpdateDetail(existing);
        return success;
    }
}
