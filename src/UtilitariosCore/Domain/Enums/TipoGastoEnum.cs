namespace UtilitariosCore.Domain.Enums;

public enum TipoGasto
{
    Otros = 0,
    Alimentacion = 1,
    Transporte = 2,
    Entretenimiento = 3,
    Servicios = 4,
    Golosina = 5,
    Salud = 6,
    Limpieza = 7,
    PrestamoDeuda = 8,      // Dinero que prestas (sale de tu bolsillo) - NEGATIVO
    Pago = 9,               // Ingresos/Sueldos que recibes - POSITIVO  
    PrestamoPago = 10       // Dinero que te devuelven de préstamos - POSITIVO
}