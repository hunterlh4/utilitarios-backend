using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetActressAdultDetailByIdQuery(int Id) : IRequest<Result<ActressAdultDetailDto>>;

internal sealed class GetActressAdultDetailByIdQueryHandler(
    IActressAdultRepository actressAdultRepository,
    IVideoAdultRepository videoAdultRepository) 
    : IRequestHandler<GetActressAdultDetailByIdQuery, Result<ActressAdultDetailDto>>
{
    public async Task<Result<ActressAdultDetailDto>> Handle(GetActressAdultDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressAdultRepository.GetActressAdultById(request.Id);
        
        if (actress == null)
        {
            return Errors.NotFound("Actress not found.");
        }

        // Una sola consulta que trae videos ya agrupados con sus actrices
        var videosGrouped = await videoAdultRepository.GetVideoAdultsWithActressesByActressId(request.Id);

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

        var result = new ActressAdultDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Videos = videoList
        };

        return result;
    }
}
