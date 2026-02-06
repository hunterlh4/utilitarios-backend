using MediatR;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record GetAllAnimeGaleriesQuery : IRequest<Result<IEnumerable<AnimeGaleryDto>>>;

internal sealed class GetAllAnimeGaleriesQueryHandler(
    IAnimeGaleryRepository repository,
    IMediaRepository mediaRepository) 
    : IRequestHandler<GetAllAnimeGaleriesQuery, Result<IEnumerable<AnimeGaleryDto>>>
{
    public async Task<Result<IEnumerable<AnimeGaleryDto>>> Handle(GetAllAnimeGaleriesQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAnimeGaleries();
        var result = new List<AnimeGaleryDto>();

        foreach (var item in items)
        {
            var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.AnimeGalery);
            var firstImage = media.OrderBy(m => m.OrderIndex).FirstOrDefault();

            result.Add(new AnimeGaleryDto
            {
                Id = item.Id,
                Name = item.Name,
                FirstImageUrl = firstImage?.Url,
                CreatedAt = item.CreatedAt
            });
        }

        return result.ToList();
    }
}
