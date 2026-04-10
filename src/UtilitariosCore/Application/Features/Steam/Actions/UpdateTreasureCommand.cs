using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record UpdateTreasureCommand (int Id) : IRequest<Result>
{
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public string? ImagePresentation { get; init; }
    public int Year { get; init; }
    public TreasureType? Type { get; init; }
}

internal sealed class UpdateTreasureCommandHandler(ISteamRepository repository)
    : IRequestHandler<UpdateTreasureCommand, Result>
{
    public async Task<Result> Handle(UpdateTreasureCommand request, CancellationToken cancellationToken)
    {
        var treasure = await repository.GetByIdTreasure(request.Id);
        if (treasure is null) return Errors.NotFound("Cofre no encontrado.");

        treasure.Name = request.Name;
        treasure.Image = request.Image;
        treasure.ImagePresentation = request.ImagePresentation;
        treasure.Year = request.Year;
        treasure.Type = request.Type;

        await repository.UpdateTreasure(treasure);
        return Results.NoContent();
    }
}
