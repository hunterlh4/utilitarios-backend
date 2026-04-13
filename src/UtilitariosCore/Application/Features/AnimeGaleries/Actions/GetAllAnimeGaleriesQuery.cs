using MediatR;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record GetAllAnimeGaleriesQuery : IRequest<Result<IEnumerable<AnimeGaleryDto>>>;

internal sealed class GetAllAnimeGaleriesQueryHandler(IGaleryRepository repository)
    : IRequestHandler<GetAllAnimeGaleriesQuery, Result<IEnumerable<AnimeGaleryDto>>>
{
    public async Task<Result<IEnumerable<AnimeGaleryDto>>> Handle(GetAllAnimeGaleriesQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAnimeGaleries();
        return items.Select(item => new AnimeGaleryDto
        {
            Id = item.Id,
            Name = item.Name,
            Image = item.Image,
            CreatedAt = item.CreatedAt
        }).ToList();
    }
}
