using Azure;
using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record GetAllActressesQuery : IRequest<Result<IEnumerable<ActressJavDto>>>;

internal sealed class GetAllActressesQueryHandler(IActressJavRepository actressRepository, ILinkRepository linkRepository)
    : IRequestHandler<GetAllActressesQuery, Result<IEnumerable<ActressJavDto>>>
{
    public async Task<Result<IEnumerable<ActressJavDto>>> Handle(GetAllActressesQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await actressRepository.GetAllActressJavWithFirstImage()).ToList();
        
        foreach (var actress in actresses)
        {
            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressJav);
            actress.Links = links
                .OrderBy(l => l.OrderIndex ?? int.MaxValue)
                .ToList();
        }

        return actresses;
    }
}
