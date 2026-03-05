using MediatR;
using UtilitariosCore.Application.Features.Events.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Events.Actions;

public record GetEventsByMonthQuery(int Year, int Month) : IRequest<Result<IEnumerable<EventDto>>>;

internal sealed class GetEventsByMonthQueryHandler(IGoogleCalendarService calendarService)
    : IRequestHandler<GetEventsByMonthQuery, Result<IEnumerable<EventDto>>>
{
    public async Task<Result<IEnumerable<EventDto>>> Handle(GetEventsByMonthQuery request, CancellationToken cancellationToken)
    {
        if (request.Month < 1 || request.Month > 12)
            return Errors.BadRequest("El mes debe estar entre 1 y 12.");

        var events = await calendarService.GetEventsByMonthAsync(request.Year, request.Month);
        return events.ToList();
    }
}
