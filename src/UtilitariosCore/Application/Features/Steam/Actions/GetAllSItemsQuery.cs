using MediatR;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record GetAllSItemsQuery : IRequest<Result<IEnumerable<SteamItemDto>>>;

internal sealed class GetAllSteamItemsQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllSItemsQuery, Result<IEnumerable<SteamItemDto>>>
{
    public async Task<Result<IEnumerable<SteamItemDto>>> Handle(GetAllSItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllItems();
        return items.Select(i => new SteamItemDto
        {
            Id = i.Id,
            ExternalId = i.ExternalId,
            Name = i.Name, 
            Image = i.Image, 
            Price = i.Price,
            Game = i.Game, 
            MarketUrl = i.MarketUrl, 
            Status = i.Status, 
            CreatedAt = i.CreatedAt
        }).ToList();
    }
}
