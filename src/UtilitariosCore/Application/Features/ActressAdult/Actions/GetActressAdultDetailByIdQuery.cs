using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetActressAdultDetailByIdQuery(int Id) : IRequest<Result<ActressAdultDetailDto>>;

internal sealed class GetActressAdultDetailByIdQueryHandler(
    IActressAdultRepository actressAdultRepository,
    IVideoAdultRepository videoAdultRepository,
    ITagRepository tagRepository,
    ILinkRepository linkRepository,
    IMediaRepository mediaRepository) 
    : IRequestHandler<GetActressAdultDetailByIdQuery, Result<ActressAdultDetailDto>>
{
    public async Task<Result<ActressAdultDetailDto>> Handle(GetActressAdultDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressAdultRepository.GetActressAdultById(request.Id);
        
        if (actress == null)
        {
            return Errors.NotFound("Actress not found.");
        }

        // Obtener imagen principal
        var media = await mediaRepository.GetMediaByRefId(request.Id, MediaType.ActressAdult);
        var mainImage = media.OrderBy(m => m.OrderIndex).FirstOrDefault();

        // Obtener tags de la actriz
        var actressTags = await tagRepository.GetTagsByRefId(request.Id, TagType.ActressAdult);

        // Obtener links de la actriz
        var links = await linkRepository.GetLinksByRefId(request.Id, LinkType.ActressAdult);

        // Obtener videos con sus actrices
        var videosGrouped = await videoAdultRepository.GetVideoAdultsWithActressesByActressId(request.Id);

        var videoList = new List<VideoAdultDto>();
        
        foreach (var v in videosGrouped)
        {
            // Obtener tags del video
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

        var result = new ActressAdultDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Image = mainImage?.Url,
            CreatedAt = actress.CreatedAt,
            Tags = actressTags.Select(t => t.Name).ToList(),
            Links = links.Select(l => new LinkDto
            {
                Id = l.Id,
                Url = l.Url,
                Name = l.Name,
                OrderIndex = l.OrderIndex
            }).ToList(),
            Videos = videoList
        };

        return result;
    }
}
