using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record UpdateSteamItemPurchaseCommand(
    int Id,
    int SteamItemId,
    decimal PurchasePrice,
    decimal SalePrice,
    decimal? Profit,
    PurchaseStatus Status,
    DateTime PurchaseDate,
    DateTime? SaleDate
) : IRequest<Result>;

internal sealed class UpdateSteamItemPurchaseCommandHandler(
    ISteamItemPurchaseRepository repository,
    ISteamItemRepository steamItemRepository)
    : IRequestHandler<UpdateSteamItemPurchaseCommand, Result>
{
    public async Task<Result> Handle(UpdateSteamItemPurchaseCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Compra no encontrada.");

        var itemExists = await steamItemRepository.Exists(request.SteamItemId);
        if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

        var purchase = new Domain.Models.SteamItemPurchase
        {
            Id = request.Id, SteamItemId = request.SteamItemId, PurchasePrice = request.PurchasePrice,
            SalePrice = request.SalePrice, Profit = request.Profit, Status = request.Status,
            PurchaseDate = request.PurchaseDate, SaleDate = request.SaleDate, CreatedAt = DateTime.Now
        };
        await repository.Update(purchase);
        return Results.NoContent();
    }
}
