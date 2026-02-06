using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Payment
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
