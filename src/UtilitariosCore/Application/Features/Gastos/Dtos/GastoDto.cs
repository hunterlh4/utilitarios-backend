using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Gastos.Dtos;

public class GastoDto
{
    public long Id { get; set; }
    public TipoGasto TipoGastoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGastoDto
{
    public TipoGasto TipoGastoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
}

public class UpdateGastoDto
{
    public TipoGasto TipoGastoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
}

public class GastoReporteDto
{
    public TipoGasto TipoGastoId { get; set; }
    public decimal TotalMonto { get; set; }
    public int CantidadRegistros { get; set; }
    public decimal MontoPromedio { get; set; }
}

public class ResumenGastosDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal TotalGastos { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal Balance { get; set; }
    public List<GastoReporteDto> GastosPorTipo { get; set; } = new();
    public List<GastoDto> UltimosGastos { get; set; } = new();
}