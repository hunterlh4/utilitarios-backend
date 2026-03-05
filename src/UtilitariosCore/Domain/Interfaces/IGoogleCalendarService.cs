using UtilitariosCore.Application.Features.Events.Dtos;

namespace UtilitariosCore.Domain.Interfaces;

public interface IGoogleCalendarService
{
    Task<EventDto> CreateEventAsync(CreateEventDto dto);
    Task<IEnumerable<EventDto>> GetEventsByMonthAsync(int year, int month);
    Task<bool> DeleteEventAsync(string eventId);
}
