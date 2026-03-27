using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record UpdateSteamItemPurchaseCommand(int Id) : IRequest<Result>
{
    public int SteamItemId { get; init; }
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
}

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

        var isSold = request.SalePrice > 0;
        var purchase = new Domain.Models.SteamItemPurchase
        {
            Id = request.Id,
            SteamItemId = request.SteamItemId,
            PurchasePrice = request.PurchasePrice,
            SalePrice = request.SalePrice,
            Profit = isSold ? request.SalePrice - request.PurchasePrice : null,
            Status = isSold ? PurchaseStatus.Vendido : PurchaseStatus.Comprado,
            PurchaseDate = DateTime.Now,
            SaleDate = isSold ? DateTime.Now : null,
            CreatedAt = DateTime.Now
        };
        await repository.Update(purchase);
        return Results.NoContent();
    }
}
