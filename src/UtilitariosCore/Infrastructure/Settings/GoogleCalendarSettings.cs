namespace UtilitariosCore.Infrastructure.Settings;

public class GoogleCalendarSettings
{
    public string ServiceAccountPath { get; set; } = string.Empty;
    public string CalendarId { get; set; } = "primary";
    public string ApplicationName { get; set; } = "UtilitariosApi";
    public string TimeZone { get; set; } = "America/Lima";
}
