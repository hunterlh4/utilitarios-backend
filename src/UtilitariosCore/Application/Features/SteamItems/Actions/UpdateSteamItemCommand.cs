using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record UpdateSteamItemCommand(
    int Id,
    string Name,
    string Image,
    string? Price,
    GameType Game,
    string MarketUrl,
    SteamItemStatus Status
) : IRequest<Result>;

internal sealed class UpdateSteamItemCommandHandler(ISteamItemRepository repository)
    : IRequestHandler<UpdateSteamItemCommand, Result>
{
    public async Task<Result> Handle(UpdateSteamItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetById(request.Id);
        if (item is null) return Errors.NotFound("Item no encontrado.");

        item.Name = request.Name;
        item.Image = request.Image;
        item.Price = request.Price;
        item.Game = request.Game;
        item.MarketUrl = request.MarketUrl;
        item.Status = request.Status;

        await repository.Update(item);
        return Results.NoContent();
    }
}
