using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record CreatePaymentCommand : IRequest<Result<int>>
{
    public string PersonName { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public sealed class Validator : AbstractValidator<CreatePaymentCommand>
    {
        public Validator()
        {
            RuleFor(x => x.PersonName).NotEmpty().WithMessage("El nombre de la persona es requerido.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(IPaymentRepository paymentRepository)
        : IRequestHandler<CreatePaymentCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                PersonName = request.PersonName.Trim(),
                Amount = request.Amount,
                CreatedAt = DateTime.Now
            };

            var paymentId = await paymentRepository.CreatePayment(payment);

            // Crear el primer detalle automáticamente como deuda
            await paymentRepository.AddDetail(new PaymentDetail
            {
                PaymentId = paymentId,
                Type = 1, // deuda (+)
                Amount = request.Amount,
                Date = DateTime.Now.Date,
                Description = "Deuda inicial",
                CreatedAt = DateTime.Now
            });

            return paymentId;
        }
    }
}
