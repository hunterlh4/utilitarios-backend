using MediatR;
using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record GetAllItemDropsQuery : IRequest<Result<IEnumerable<SteamItemDropDto>>>;

internal sealed class GetAllSteamItemDropsQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllItemDropsQuery, Result<IEnumerable<SteamItemDropDto>>>
{
    public async Task<Result<IEnumerable<SteamItemDropDto>>> Handle(GetAllItemDropsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllDrops();
        return items.Select(item => new SteamItemDropDto
        {
            Id = item.Id,
            Quantity = item.Quantity,
            Price = item.Price,
            SalePrice = item.SalePrice,
            Total = item.Total,
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
