namespace UtilitariosCore.Domain.Models;

public class Payment
{
    public int Id { get; set; }
    public required string PersonName { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaymentDetail
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public int Type { get; set; } // 1: deuda(+), 2: pago(-), 3: interes_deuda(+), 4: interes_pago(-)
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
