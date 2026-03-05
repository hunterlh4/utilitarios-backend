using MediatR;
using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record GetAllSteamItemPurchasesQuery : IRequest<Result<IEnumerable<SteamItemPurchaseDto>>>;

internal sealed class GetAllSteamItemPurchasesQueryHandler(ISteamItemPurchaseRepository repository)
    : IRequestHandler<GetAllSteamItemPurchasesQuery, Result<IEnumerable<SteamItemPurchaseDto>>>
{
    public async Task<Result<IEnumerable<SteamItemPurchaseDto>>> Handle(GetAllSteamItemPurchasesQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAll();
        return items.ToList();
    }
}
