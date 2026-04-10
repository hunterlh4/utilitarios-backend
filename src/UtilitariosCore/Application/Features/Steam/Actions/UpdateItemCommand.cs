using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record UpdateItemCommand(int Id) : IRequest<Result>
{
    public string? ExternalId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public GameType Game { get; init; }
    public string MarketUrl { get; init; } = string.Empty;
    public SteamItemStatus Status { get; init; }
}

internal sealed class UpdateSteamItemCommandHandler(ISteamRepository repository)
    : IRequestHandler<UpdateItemCommand, Result>
{
    public async Task<Result> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdItems(request.Id);
        if (item is null) return Errors.NotFound("Item no encontrado.");

        item.ExternalId = request.ExternalId;
        item.Name = request.Name;
        item.Image = request.Image;
        item.Price = request.Price;
        item.Game = request.Game;
        item.MarketUrl = request.MarketUrl;
        item.Status = request.Status;

        await repository.UpdateItems(item);
        return Results.NoContent();
    }
}
