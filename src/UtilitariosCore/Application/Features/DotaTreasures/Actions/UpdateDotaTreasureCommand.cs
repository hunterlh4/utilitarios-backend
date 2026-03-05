using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record UpdateDotaTreasureCommand(
    int Id,
    string Name,
    string Image,
    string? ImagePresentation,
    int Year,
    TreasureType? Type
) : IRequest<Result>;

internal sealed class UpdateDotaTreasureCommandHandler(IDotaTreasureRepository repository)
    : IRequestHandler<UpdateDotaTreasureCommand, Result>
{
    public async Task<Result> Handle(UpdateDotaTreasureCommand request, CancellationToken cancellationToken)
    {
        var treasure = await repository.GetById(request.Id);
        if (treasure is null) return Errors.NotFound("Cofre no encontrado.");

        treasure.Name = request.Name;
        treasure.Image = request.Image;
        treasure.ImagePresentation = request.ImagePresentation;
        treasure.Year = request.Year;
        treasure.Type = request.Type;

        await repository.Update(treasure);
        return Results.NoContent();
    }
}
