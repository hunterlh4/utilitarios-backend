using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetActressAdultByIdQuery(int Id) : IRequest<Result<ActressAdultDetailDto>>;

internal sealed class GetActressAdultByIdQueryHandler(
    IActressAdultRepository actressAdultRepository,
    IVideoAdultRepository videoAdultRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetActressAdultByIdQuery, Result<ActressAdultDetailDto>>
{
    public async Task<Result<ActressAdultDetailDto>> Handle(GetActressAdultByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressAdultRepository.GetActressAdultWithTagsAndImageById(request.Id);
        if (actress == null) return Errors.NotFound("Actriz no encontrada.");

        var videosGrouped = await videoAdultRepository.GetVideoAdultsWithActressesByActressId(request.Id);
        var links = await linkRepository.GetLinksByRefId(request.Id, LinkType.ActressAdult);

        var videoList = new List<VideoAdultDto>();
        foreach (var v in videosGrouped)
        {
            var videoTags = await tagRepository.GetTagsByRefId(v.VideoId, TagType.VideoAdult);
            videoList.Add(new VideoAdultDto
            {
                Id = v.VideoId,
                Source = v.Source,
                VideoUrl = v.VideoUrl,
                Title = v.Title,
                ThumbnailUrl = v.ThumbnailUrl,
                Status = v.Status,
                Actresses = v.Actresses.Select(a => new ActressSimpleDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                Tags = videoTags.Select(t => t.Name).ToList(),
                CreatedAt = v.VideoCreatedAt
            });
        }

        return new ActressAdultDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Image = actress.Image,
            CreatedAt = actress.CreatedAt,
            Tags = actress.Tags,
            Links = links.Select(l => new LinkDto
            {
                Id = l.Id,
                Name = l.Name,
                Url = l.Url,
                OrderIndex = l.OrderIndex
            }).ToList(),
            Videos = videoList
        };
    }
}
