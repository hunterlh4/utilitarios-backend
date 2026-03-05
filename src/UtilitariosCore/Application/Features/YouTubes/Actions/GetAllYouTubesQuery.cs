using MediatR;
using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.YouTubes.Actions;

public record GetAllYouTubesQuery(YouTubeCategory? Category = null) : IRequest<Result<IEnumerable<YouTubeDto>>>;

internal sealed class GetAllYouTubesQueryHandler(IYouTubeRepository youTubeRepository)
    : IRequestHandler<GetAllYouTubesQuery, Result<IEnumerable<YouTubeDto>>>
{
    public async Task<Result<IEnumerable<YouTubeDto>>> Handle(GetAllYouTubesQuery request, CancellationToken cancellationToken)
    {
        var result = await youTubeRepository.GetAll(request.Category);
        return result.ToList();
    }
}
