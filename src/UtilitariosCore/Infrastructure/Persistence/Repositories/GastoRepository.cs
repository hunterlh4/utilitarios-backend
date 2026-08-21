using Dapper;
using UtilitariosCore.Application.Features.Gastos.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class GastoRepository(MssqlContext context) : IGastoRepository
{
    public async Task<long> CreateGasto(Gasto gasto)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Gastos (TipoGastoId, Monto, Fecha, Descripcion, CreatedAt)
        VALUES (@TipoGastoId, @Monto, @Fecha, @Descripcion, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";

        var result = await db.QuerySingleAsync<long>(sql, gasto);
        return result;
    }

    public async Task<bool> UpdateGasto(Gasto gasto)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Gastos
        SET TipoGastoId = @TipoGastoId, 
            Monto = @Monto, 
            Fecha = @Fecha, 
            Descripcion = @Descripcion
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, gasto);
        return result > 0;
    }

    public async Task<bool> DeleteGasto(long id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Gastos WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Gasto?> GetGastoById(long id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, TipoGastoId, Monto, Fecha, Descripcion, CreatedAt FROM Gastos WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Gasto>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<GastoDto>> GetGastosByDateRange(DateTime fechaInicio, DateTime fechaFin)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT 
            Id, 
            TipoGastoId, 
            Monto, 
            Fecha, 
            Descripcion, 
            CreatedAt
        FROM Gastos 
        WHERE Fecha >= @FechaInicio AND Fecha <= @FechaFin
        ORDER BY Fecha DESC, CreatedAt DESC
        ";

        var result = await db.QueryAsync<GastoDto>(sql, new { FechaInicio = fechaInicio, FechaFin = fechaFin });
        return result;
    }

    public async Task<IEnumerable<GastoReporteDto>> GetGastosReportByDateRange(DateTime fechaInicio, DateTime fechaFin)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT 
            TipoGastoId,
            SUM(Monto) AS TotalMonto,
            COUNT(*) AS CantidadRegistros,
            AVG(Monto) AS MontoPromedio
        FROM Gastos 
        WHERE Fecha >= @FechaInicio AND Fecha <= @FechaFin
        GROUP BY TipoGastoId
        ORDER BY TotalMonto DESC
        ";

        var result = await db.QueryAsync<GastoReporteDto>(sql, new { FechaInicio = fechaInicio, FechaFin = fechaFin });
        return result;
    }

    public async Task<ResumenGastosDto> GetResumenGastos(DateTime fechaInicio, DateTime fechaFin)
    {
        var db = context.CreateDefaultConnection();

        // Obtener totales generales
        string sqlTotales = @"
        SELECT 
            SUM(CASE WHEN TipoGastoId IN (9, 10) THEN Monto ELSE 0 END) AS TotalIngresos,
            SUM(CASE WHEN TipoGastoId NOT IN (9, 10) THEN ABS(Monto) ELSE 0 END) AS TotalGastos
        FROM Gastos 
        WHERE Fecha >= @FechaInicio AND Fecha <= @FechaFin
        ";

        var totales = await db.QueryFirstAsync<(decimal TotalIngresos, decimal TotalGastos)>(
            sqlTotales, 
            new { FechaInicio = fechaInicio, FechaFin = fechaFin }
        );

        // Obtener reporte por tipos
        var gastosPorTipo = await GetGastosReportByDateRange(fechaInicio, fechaFin);

        // Obtener últimos 10 gastos del período
        string sqlUltimos = @"
        SELECT TOP 10
            Id, 
            TipoGastoId, 
            Monto, 
            Fecha, 
            Descripcion, 
            CreatedAt
        FROM Gastos 
        WHERE Fecha >= @FechaInicio AND Fecha <= @FechaFin
        ORDER BY CreatedAt DESC
        ";

        var ultimosGastos = await db.QueryAsync<GastoDto>(sqlUltimos, new { FechaInicio = fechaInicio, FechaFin = fechaFin });

        return new ResumenGastosDto
        {
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            TotalGastos = totales.TotalGastos,
            TotalIngresos = totales.TotalIngresos,
            Balance = totales.TotalIngresos - totales.TotalGastos,
            GastosPorTipo = gastosPorTipo.ToList(),
            UltimosGastos = ultimosGastos.ToList()
        };
    }

    public async Task<IEnumerable<GastoDto>> GetAllGastos()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT 
            Id, 
            TipoGastoId, 
            Monto, 
            Fecha, 
            Descripcion, 
            CreatedAt
        FROM Gastos 
        ORDER BY CreatedAt DESC
        ";

        var result = await db.QueryAsync<GastoDto>(sql);
        return result;
    }
}