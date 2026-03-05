using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record AddPaymentDetailCommand : IRequest<Result<int>>
{
    public int PaymentId { get; set; }
    public int Type { get; set; }       // 1: deuda(+), 2: pago(-), 3: interés_deuda(+), 4: interés_pago(-)
    public decimal Amount { get; set; } // siempre positivo, el tipo define el signo
    public DateTime Date { get; set; }
    public string? Description { get; set; }

    public sealed class Validator : AbstractValidator<AddPaymentDetailCommand>
    {
        public Validator()
        {
            RuleFor(x => x.PaymentId).GreaterThan(0);
            RuleFor(x => x.Type).InclusiveBetween(1, 4).WithMessage("El tipo debe ser 1 (deuda), 2 (pago), 3 (interés deuda) o 4 (interés pago).");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
            RuleFor(x => x.Date).NotEmpty();
        }
    }

    internal sealed class Handler(IPaymentRepository paymentRepository)
        : IRequestHandler<AddPaymentDetailCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AddPaymentDetailCommand request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetPaymentById(request.PaymentId);
            if (payment is null) return Errors.NotFound("Deuda no encontrada.");

            var detail = new PaymentDetail
            {
                PaymentId = request.PaymentId,
                Type = request.Type,
                Amount = request.Amount,
                Date = request.Date.Date,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };

            var id = await paymentRepository.AddDetail(detail);
            return id;
        }
    }
}
