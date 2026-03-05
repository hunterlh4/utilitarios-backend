using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record DeletePaymentCommand(int Id) : IRequest<Result>;

internal sealed class DeletePaymentCommandHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<DeletePaymentCommand, Result>
{
    public async Task<Result> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var exists = await paymentRepository.GetPaymentById(request.Id);
        if (exists is null) return Errors.NotFound("Deuda no encontrada.");

        await paymentRepository.DeletePayment(request.Id);
        return Results.NoContent();
    }
}
