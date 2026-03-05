using MediatR;
using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record GetAllSteamItemDropsQuery : IRequest<Result<IEnumerable<SteamItemDropDto>>>;

internal sealed class GetAllSteamItemDropsQueryHandler(ISteamItemDropRepository repository)
    : IRequestHandler<GetAllSteamItemDropsQuery, Result<IEnumerable<SteamItemDropDto>>>
{
    public async Task<Result<IEnumerable<SteamItemDropDto>>> Handle(GetAllSteamItemDropsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAll();
        return items.ToList();
    }
}
