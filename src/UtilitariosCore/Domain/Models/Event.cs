using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Event
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public EventType Type { get; set; }
    public bool AllDay { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}
