using UtilitariosCore.Application.Features.Payments.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<PaymentDetailDto>> GetAllPayments();
    Task<PaymentDetailDto?> GetPaymentById(int id);
    Task<int> CreatePayment(Payment payment);
    Task<bool> DeletePayment(int id);
    Task<int> AddDetail(PaymentDetail detail);
    Task<bool> UpdateDetail(PaymentDetail detail);
    Task<bool> DeleteDetail(int detailId);
    Task<PaymentDetail?> GetDetailById(int id);
}
