using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Events.Actions;

public record DeleteEventCommand(string EventId) : IRequest<Result<bool>>;

internal sealed class DeleteEventCommandHandler(IGoogleCalendarService calendarService)
    : IRequestHandler<DeleteEventCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.EventId))
            return Errors.BadRequest("El ID del evento es requerido.");

        var success = await calendarService.DeleteEventAsync(request.EventId);
        return success;
    }
}
