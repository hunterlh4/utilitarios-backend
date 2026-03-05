using MediatR;
using UtilitariosCore.Application.Features.SteamItems.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record GetAllSteamItemsQuery : IRequest<Result<IEnumerable<SteamItemDto>>>;

internal sealed class GetAllSteamItemsQueryHandler(ISteamItemRepository repository)
    : IRequestHandler<GetAllSteamItemsQuery, Result<IEnumerable<SteamItemDto>>>
{
    public async Task<Result<IEnumerable<SteamItemDto>>> Handle(GetAllSteamItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAll();
        return items.Select(i => new SteamItemDto
        {
            Id = i.Id, Name = i.Name, Image = i.Image, Price = i.Price,
            Game = i.Game, MarketUrl = i.MarketUrl, Status = i.Status, CreatedAt = i.CreatedAt
        }).ToList();
    }
}
