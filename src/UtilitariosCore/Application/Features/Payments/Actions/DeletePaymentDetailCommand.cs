using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record DeletePaymentDetailCommand(int DetailId) : IRequest<Result>;

internal sealed class DeletePaymentDetailCommandHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<DeletePaymentDetailCommand, Result>
{
    public async Task<Result> Handle(DeletePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        var exists = await paymentRepository.GetDetailById(request.DetailId);
        if (exists is null) return Errors.NotFound("Movimiento no encontrado.");

        await paymentRepository.DeleteDetail(request.DetailId);
        return Results.NoContent();
    }
}
