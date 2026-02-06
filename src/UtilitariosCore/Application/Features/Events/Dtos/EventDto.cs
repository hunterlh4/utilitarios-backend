using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Events.Dtos;

public class EventDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public EventType Type { get; set; }
    public bool AllDay { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateEventDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public EventType Type { get; set; }
    public bool AllDay { get; set; }
    public string? Color { get; set; }
}

public class UpdateEventDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public EventType? Type { get; set; }
    public bool? AllDay { get; set; }
    public string? Color { get; set; }
}
