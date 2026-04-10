using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record UpdateItemDropCommand(int Id) : IRequest<Result>
{
    public int SteamItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal SalePrice { get; init; }
}

internal sealed class UpdateItemDropCommandHandler(
    ISteamRepository repository)
    : IRequestHandler<UpdateItemDropCommand, Result>
{
    public async Task<Result> Handle(UpdateItemDropCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsDrops(request.Id);
        if (!exists) return Errors.NotFound("Drop no encontrado.");

        var itemExists = await repository.ExistsItems(request.SteamItemId);
        if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

        var drop = new SteamItemDrop
        {
            Id = request.Id,
            SteamItemId = request.SteamItemId,
            Quantity = request.Quantity,
            Price = request.Price,
            SalePrice = request.SalePrice,
            Total = request.Quantity * request.SalePrice,
            CreatedAt = DateTime.Now
        };
        await repository.UpdateDrops(drop);
        return Results.NoContent();
    }
}
