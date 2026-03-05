using MediatR;
using UtilitariosCore.Application.Features.Payments.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Payments.Actions;

public record GetAllPaymentsQuery : IRequest<Result<IEnumerable<PaymentDetailDto>>>;

internal sealed class GetAllPaymentsQueryHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<GetAllPaymentsQuery, Result<IEnumerable<PaymentDetailDto>>>
{
    public async Task<Result<IEnumerable<PaymentDetailDto>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var result = await paymentRepository.GetAllPayments();
        return result.ToList();
    }
}
