using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IGastoRepository
{
    Task<long> CreateGasto(Gasto gasto);
    Task<bool> UpdateGasto(Gasto gasto);
    Task<bool> DeleteGasto(long id);
    Task<Gasto?> GetGastoById(long id);
    Task<IEnumerable<GastoDto>> GetGastosByDateRange(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<GastoReporteDto>> GetGastosReportByDateRange(DateTime fechaInicio, DateTime fechaFin);
    Task<ResumenGastosDto> GetResumenGastos(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<GastoDto>> GetAllGastos();
}