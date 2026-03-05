using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record UpdateSteamItemDropCommand(
    int Id,
    int SteamItemId,
    int Quantity,
    decimal Price,
    decimal SalePrice,
    decimal Total
) : IRequest<Result>;

internal sealed class UpdateSteamItemDropCommandHandler(
    ISteamItemDropRepository repository,
    ISteamItemRepository steamItemRepository)
    : IRequestHandler<UpdateSteamItemDropCommand, Result>
{
    public async Task<Result> Handle(UpdateSteamItemDropCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Drop no encontrado.");

        var itemExists = await steamItemRepository.Exists(request.SteamItemId);
        if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

        var drop = new Domain.Models.SteamItemDrop
        {
            Id = request.Id, SteamItemId = request.SteamItemId, Quantity = request.Quantity,
            Price = request.Price, SalePrice = request.SalePrice, Total = request.Total,
            CreatedAt = DateTime.Now
        };
        await repository.Update(drop);
        return Results.NoContent();
    }
}
