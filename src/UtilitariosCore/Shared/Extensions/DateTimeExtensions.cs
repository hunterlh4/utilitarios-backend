namespace UtilitariosCore.Shared.Extensions;

public static class DateTimeExtensions
{
    private static string? _timeZoneId;

    public static void Initialize(string timeZoneId)
    {
        _timeZoneId = timeZoneId;
    }

    public static DateTimeOffset ToTimeZone(this DateTimeOffset dateTimeOffset)
    {
        if (string.IsNullOrWhiteSpace(_timeZoneId)) throw new InvalidOperationException("The time zone is not set");

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
        
        // Convertir a DateTime UTC primero
        var utcDateTime = dateTimeOffset.UtcDateTime;
        
        // Convertir a la zona horaria destino
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        
        // Obtener el offset correcto para esa fecha/hora
        var offset = timeZone.GetUtcOffset(localDateTime);
        
        // Crear el DateTimeOffset con la hora local y el offset correcto
        return new DateTimeOffset(localDateTime, offset);
    }

    /// <summary>
    /// Obtiene la fecha y hora actual en la zona horaria configurada con el offset correcto
    /// </summary>
    public static DateTimeOffset Now()
    {
        if (string.IsNullOrWhiteSpace(_timeZoneId)) throw new InvalidOperationException("The time zone is not set");

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
        
        // Obtener la hora actual en la zona horaria especificada
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        
        // Obtener el offset de la zona horaria
        var offset = timeZone.GetUtcOffset(localTime);
        
        // Crear DateTimeOffset con la hora local y el offset correcto
        return new DateTimeOffset(localTime, offset);
    }

    /// <summary>
    /// Convierte una fecha local (de la zona horaria configurada) a UTC
    /// Útil para filtros de búsqueda
    /// </summary>
    public static DateTimeOffset FromTimeZoneToUtc(this DateTime localDateTime)
    {
        if (string.IsNullOrWhiteSpace(_timeZoneId)) throw new InvalidOperationException("The time zone is not set");

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
        
        // Asegurar que el DateTime tenga Kind.Unspecified para que se trate como hora local
        var unspecifiedDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
        
        // Convertir la fecha local a UTC
        var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(unspecifiedDateTime, timeZone);
        
        // Retornar como DateTimeOffset en UTC
        return new DateTimeOffset(utcDateTime, TimeSpan.Zero);
    }
}
