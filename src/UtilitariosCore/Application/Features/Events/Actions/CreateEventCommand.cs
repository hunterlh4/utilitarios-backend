using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Events.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Events.Actions;

public record CreateEventCommand : IRequest<Result<EventDto>>
{
    public string Title { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public EventType Type { get; init; }
    public bool AllDay { get; init; }
    public string? Color { get; init; }
}

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("El título es requerido.");
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("La fecha de fin debe ser mayor o igual a la de inicio.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("Tipo de evento inválido.");
    }
}

internal sealed class CreateEventCommandHandler(IGoogleCalendarService calendarService)
    : IRequestHandler<CreateEventCommand, Result<EventDto>>
{
    public async Task<Result<EventDto>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var dto = new CreateEventDto
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            AllDay = request.AllDay,
            Color = request.Color
        };

        var created = await calendarService.CreateEventAsync(dto);
        return created;
    }
}
