using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Gasto
{
    public long Id { get; set; }
    public TipoGasto TipoGastoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreatedAt { get; set; }
}