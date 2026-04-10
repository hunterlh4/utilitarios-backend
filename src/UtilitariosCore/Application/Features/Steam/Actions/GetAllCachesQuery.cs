using MediatR;
using UtilitariosCore.Application.Features.DotaCaches.Dtos;
using UtilitariosCore.Application.Features.DotaHeroes.Dtos;
using UtilitariosCore.Application.Features.DotaTreasures.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record GetAllCachesQuery() : IRequest<Result<IEnumerable<DotaCacheDto>>>;

internal sealed class GetAllCachesQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllCachesQuery, Result<IEnumerable<DotaCacheDto>>>
{
    public async Task<Result<IEnumerable<DotaCacheDto>>> Handle(GetAllCachesQuery request, CancellationToken cancellationToken)
    {
        var caches = await repository.GetAllCache();

        var items = caches.Select(cache => new DotaCacheDto
        {
            Id = cache.Id,
            TreasureId = cache.TreasureId,
            HeroId = cache.HeroId,
            Name = cache.Name,
            Photo = cache.Photo,
            Price = cache.Price,
            Quantity = cache.Quantity,
            Total = cache.Total,
            Owner = cache.Owner,
            CreatedAt = cache.CreatedAt,
            Hero = cache.Hero is null ? null : new DotaHeroDto
                {
                    Id = cache.Hero.Id,
                    Name = cache.Hero.Name,
                    Image = cache.Hero.Image,
                    CreatedAt = cache.Hero.CreatedAt
                },
            Treasure = cache.Treasure is null ? null : new DotaTreasureDto
                {
                    Id = cache.Treasure.Id,
                    Name = cache.Treasure.Name,
                    Image = cache.Treasure.Image,
                    ImagePresentation = cache.Treasure.ImagePresentation,
                    Year = cache.Treasure.Year,
                    Type = cache.Treasure.Type,
                    CreatedAt = cache.Treasure.CreatedAt
                }
        });

        return items.ToList();
    }
}
