using MediatR;
using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record GetAllItemPurchasesQuery : IRequest<Result<IEnumerable<SteamItemPurchaseDto>>>;

internal sealed class GetAllSteamItemPurchasesQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllItemPurchasesQuery, Result<IEnumerable<SteamItemPurchaseDto>>>
{
    public async Task<Result<IEnumerable<SteamItemPurchaseDto>>> Handle(GetAllItemPurchasesQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllPurchase();
        return items.Select(item => new SteamItemPurchaseDto
        {
            Id = item.Id,
            PurchasePrice = item.PurchasePrice,
            SalePrice = item.SalePrice,
            Profit = item.Profit,
            Status = item.Status,
            CreatedAt = item.CreatedAt,
            Item = new SteamItemRefDto
            {
                Id = item.SteamItemId,
                Name = item.ItemName ?? string.Empty,
                Image = item.ItemImage ?? string.Empty,
                MarketUrl = item.ItemMarketUrl ?? string.Empty,
                Game = item.ItemGame,
            }
        }).ToList();
    }
}
