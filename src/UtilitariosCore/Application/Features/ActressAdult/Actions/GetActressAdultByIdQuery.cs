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
    ILinkRepository linkRepository)
    : IRequestHandler<GetActressAdultByIdQuery, Result<ActressAdultDetailDto>>
{
    public async Task<Result<ActressAdultDetailDto>> Handle(GetActressAdultByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressAdultRepository.GetActressAdultById(request.Id);
        if (actress == null) return Errors.NotFound("Actriz no encontrada.");

        var videosGrouped = await videoAdultRepository.GetVideoAdultsWithActressesByActressId(request.Id);
        var links = await linkRepository.GetLinksByRefId(request.Id, LinkType.ActressAdult);

        var videoList = videosGrouped.Select(v => new VideoAdultDto
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
            CreatedAt = v.VideoCreatedAt
        }).ToList();

        return new ActressAdultDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
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
