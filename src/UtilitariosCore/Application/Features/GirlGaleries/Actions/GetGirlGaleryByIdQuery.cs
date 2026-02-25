using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record GetGirlGaleryByIdQuery(int Id) : IRequest<Result<GirlGaleryDetailDto>>;

internal sealed class GetGirlGaleryByIdQueryHandler(
    IGirlGaleryRepository repository,
    IMediaRepository mediaRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<GetGirlGaleryByIdQuery, Result<GirlGaleryDetailDto>>
{
    public async Task<Result<GirlGaleryDetailDto>> Handle(GetGirlGaleryByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetGirlGaleryById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.GirlGalery);
        var mediaList = media
            .OrderBy(m => m.OrderIndex)
            .Skip(1)
            .Select(m => new MediaDto
            {
                Id = m.Id,
                Url = m.Url,
                Thumbnail = m.Thumbnail,
                OrderIndex = m.OrderIndex
            })
            .ToList();

        var links = await linkRepository.GetLinksByRefId(item.Id, LinkType.GirlGalery);
        var linkList = links.Select(l => new GirlGaleryLinkDto
        {
            Id = l.Id,
            Name = l.Name,
            Url = l.Url,
            OrderIndex = l.OrderIndex
        }).ToList();

        return new GirlGaleryDetailDto
        {
            Id = item.Id,
            Name = item.Name,
            Media = mediaList,
            Links = linkList,
            CreatedAt = item.CreatedAt
        };
    }
}
