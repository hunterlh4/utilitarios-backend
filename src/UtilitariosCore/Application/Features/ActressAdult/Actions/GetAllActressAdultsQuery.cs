using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetAllActressAdultsQuery : IRequest<Result<IEnumerable<ActressAdultDto>>>;

internal sealed class GetAllActressAdultsQueryHandler(IActressAdultRepository actressAdultRepository, ILinkRepository linkRepository)
    : IRequestHandler<GetAllActressAdultsQuery, Result<IEnumerable<ActressAdultDto>>>
{
    public async Task<Result<IEnumerable<ActressAdultDto>>> Handle(GetAllActressAdultsQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await actressAdultRepository.GetAllActressAdultsWithFirstImage()).ToList();

        foreach (var actress in actresses)
        {
            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressAdult);
            actress.Links = links
                .OrderBy(l => l.OrderIndex ?? int.MaxValue)
                .ToList();
        }

        return actresses;
    }
}
