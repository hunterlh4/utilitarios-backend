using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Payments.Dtos;

public class PaymentDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePaymentDto
{
    public int PersonId { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
}

public class UpdatePaymentDto
{
    public int? PersonId { get; set; }
    public PaymentType? Type { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateOnly? Date { get; set; }
}
