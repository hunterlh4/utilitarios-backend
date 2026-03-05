using MediatR;
using UtilitariosCore.Application.Features.DotaCaches.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record GetAllDotaCachesQuery(int? TreasureId = null) : IRequest<Result<IEnumerable<DotaCacheDto>>>;

internal sealed class GetAllDotaCachesQueryHandler(IDotaCacheRepository repository)
    : IRequestHandler<GetAllDotaCachesQuery, Result<IEnumerable<DotaCacheDto>>>
{
    public async Task<Result<IEnumerable<DotaCacheDto>>> Handle(GetAllDotaCachesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<DotaCacheDto> items;

        if (request.TreasureId.HasValue)
            items = await repository.GetByTreasureId(request.TreasureId.Value);
        else
            items = await repository.GetAll();

        return items.ToList();
    }
}
