using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using UtilitariosCore.Application.Features.Events.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Infrastructure.Settings;

namespace UtilitariosCore.Infrastructure.Services.GoogleCalendar;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly CalendarService _service;
    private readonly GoogleCalendarSettings _settings;

    public GoogleCalendarService(IOptions<GoogleCalendarSettings> options)
    {
        _settings = options.Value;

        var credential = GoogleCredential
            .FromFile(_settings.ServiceAccountPath)
            .CreateScoped(CalendarService.Scope.Calendar);

        _service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _settings.ApplicationName
        });
    }

    public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
    {
        var googleEvent = new Event
        {
            Summary = dto.Title,
            ColorId = dto.Color,
            ExtendedProperties = new Event.ExtendedPropertiesData
            {
                Private__ = new Dictionary<string, string>
                {
                    { "type", ((int)dto.Type).ToString() },
                    { "allDay", dto.AllDay.ToString() }
                }
            }
        };

        if (dto.AllDay)
        {
            googleEvent.Start = new EventDateTime { Date = dto.StartDate.ToString("yyyy-MM-dd") };
            // Google Calendar: EndDate para AllDay es exclusivo (día siguiente)
            googleEvent.End = new EventDateTime { Date = dto.EndDate.AddDays(1).ToString("yyyy-MM-dd") };
        }
        else
        {
            googleEvent.Start = new EventDateTime
            {
                DateTimeDateTimeOffset = dto.StartDate.ToDateTime(TimeOnly.MinValue),
                TimeZone = _settings.TimeZone
            };
            googleEvent.End = new EventDateTime
            {
                DateTimeDateTimeOffset = dto.EndDate.ToDateTime(new TimeOnly(23, 59, 59)),
                TimeZone = _settings.TimeZone
            };
        }

        var created = await _service.Events.Insert(googleEvent, _settings.CalendarId).ExecuteAsync();

        return MapToDto(created);
    }

    public async Task<IEnumerable<EventDto>> GetEventsByMonthAsync(int year, int month)
    {
        var timeMin = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Local);
        var timeMax = timeMin.AddMonths(1);

        var request = _service.Events.List(_settings.CalendarId);
        request.TimeMinDateTimeOffset = timeMin;
        request.TimeMaxDateTimeOffset = timeMax;
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var response = await request.ExecuteAsync();

        return response.Items?.Select(MapToDto) ?? [];
    }

    public async Task<bool> DeleteEventAsync(string eventId)
    {
        await _service.Events.Delete(_settings.CalendarId, eventId).ExecuteAsync();
        return true;
    }

    private static EventDto MapToDto(Event e)
    {
        var props = e.ExtendedProperties?.Private__ ?? new Dictionary<string, string>();

        // StartDate: puede ser AllDay (Date) o con hora (DateTimeDateTimeOffset)
        var startDate = e.Start.Date is not null
            ? DateOnly.ParseExact(e.Start.Date, "yyyy-MM-dd")
            : DateOnly.FromDateTime(e.Start.DateTimeDateTimeOffset?.DateTime ?? DateTime.Now);

        // EndDate: para AllDay Google devuelve el día siguiente (exclusivo), restar 1
        DateOnly endDate;
        if (e.End.Date is not null)
        {
            var raw = DateOnly.ParseExact(e.End.Date, "yyyy-MM-dd");
            var isAllDay = !props.TryGetValue("allDay", out var ad) || ad != "False";
            endDate = isAllDay ? raw.AddDays(-1) : raw;
        }
        else
        {
            endDate = DateOnly.FromDateTime(e.End.DateTimeDateTimeOffset?.DateTime ?? DateTime.Now);
        }

        _ = int.TryParse(props.TryGetValue("type", out var typeStr) ? typeStr : "2", out var typeInt);
        var type = Enum.IsDefined(typeof(EventType), typeInt) ? (EventType)typeInt : EventType.Personal;
        var allDay = !props.TryGetValue("allDay", out var allDayStr) || allDayStr != "False";

        return new EventDto
        {
            Id = e.Id ?? string.Empty,
            Title = e.Summary ?? string.Empty,
            StartDate = startDate,
            EndDate = endDate,
            Type = type,
            AllDay = allDay,
            Color = e.ColorId,
            CreatedAt = e.Created ?? DateTime.Now
        };
    }
}
