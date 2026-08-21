using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Helpers;

public static class TipoGastoHelper
{
    public static bool EsIngreso(TipoGasto tipo) => tipo switch
    {
        TipoGasto.Pago => true,              // Sueldos, ingresos
        TipoGasto.PrestamoPago => true,      // Dinero que te devuelven
        _ => false                           // Todos los demás son gastos (negativos)
    };

    public static decimal ConvertirMontoSegunTipo(decimal monto, TipoGasto tipo)
    {
        var esIngreso = EsIngreso(tipo);
        
        if (esIngreso)
        {
            // Para ingresos, debe ser positivo
            return Math.Abs(monto);
        }
        else
        {
            // Para gastos, debe ser negativo
            return monto > 0 ? -monto : monto; // Solo convertir si viene positivo
        }
    }
}