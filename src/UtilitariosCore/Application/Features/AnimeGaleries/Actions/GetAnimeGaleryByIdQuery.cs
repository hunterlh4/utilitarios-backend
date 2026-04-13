using MediatR;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record GetAnimeGaleryByIdQuery(int Id) : IRequest<Result<AnimeGaleryDetailDto>>;

internal sealed class GetAnimeGaleryByIdQueryHandler(
    IGaleryRepository repository,
    IMediaRepository mediaRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<GetAnimeGaleryByIdQuery, Result<AnimeGaleryDetailDto>>
{
    public async Task<Result<AnimeGaleryDetailDto>> Handle(GetAnimeGaleryByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetAnimeGaleryById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.AnimeGalery);
        var mediaList = media
            .OrderBy(m => m.OrderIndex)
            .Select(m => new MediaDto
            {
                Id = m.Id,
                Url = m.Url,
                Thumbnail = m.Thumbnail,
                OrderIndex = m.OrderIndex
            })
            .ToList();

        var links = await linkRepository.GetLinksByRefId(item.Id, LinkType.AnimeGalery);
        var linkList = links.Select(l => new AnimeGaleryLinkDto
        {
            Id = l.Id,
            Name = l.Name,
            Url = l.Url,
            OrderIndex = l.OrderIndex
        }).ToList();

        var result = new AnimeGaleryDetailDto
        {
            Id = item.Id,
            Name = item.Name,
            Image = item.Image,
            Media = mediaList,
            Links = linkList,
            CreatedAt = item.CreatedAt
        };

        return result;
    }
}
