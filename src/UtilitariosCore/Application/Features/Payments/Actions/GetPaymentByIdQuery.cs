using MediatR;
using UtilitariosCore.Application.Features.Payments.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record GetPaymentByIdQuery(int Id) : IRequest<Result<PaymentDetailDto>>;

internal sealed class GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDetailDto>>
{
    public async Task<Result<PaymentDetailDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await paymentRepository.GetPaymentById(request.Id);
        if (result is null) return Errors.NotFound("Deuda no encontrada.");
        return result;
    }
}
