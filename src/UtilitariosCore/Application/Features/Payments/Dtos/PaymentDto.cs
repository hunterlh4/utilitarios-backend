namespace UtilitariosCore.Application.Features.Payments.Dtos;

// Tipos de movimiento: 1=deuda(+), 2=pago(-), 3=interés_deuda(+), 4=interés_pago(-)
// El usuario solo manda el monto positivo — el tipo determina si suma o resta

public class PaymentListDto
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public decimal Amount { get; set; }         // monto inicial de la deuda
    public decimal Balance { get; set; }        // saldo actual calculado
    public DateTime CreatedAt { get; set; }
}

public class PaymentDetailItemDto
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public int Type { get; set; }               // 1: deuda(+), 2: pago(-), 3: interés_deuda(+), 4: interés_pago(-)
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaymentDetailDto
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public decimal Amount { get; set; }         // monto inicial
    public decimal Balance { get; set; }        // saldo actual
    public DateTime CreatedAt { get; set; }
    public List<PaymentDetailItemDto> Details { get; set; } = [];
}
