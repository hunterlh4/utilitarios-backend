using MediatR;
using UtilitariosCore.Application.Features.DotaHeroes.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record GetAllHeroesQuery : IRequest<Result<IEnumerable<DotaHeroDto>>>;

internal sealed class GetAllHeroesQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllHeroesQuery, Result<IEnumerable<DotaHeroDto>>>
{
    public async Task<Result<IEnumerable<DotaHeroDto>>> Handle(GetAllHeroesQuery request, CancellationToken cancellationToken)
    {
        var heroes = await repository.GetAllHero();
        return heroes.Select(h => new DotaHeroDto
        {
            Id = h.Id, 
            Name = h.Name, 
            Image = h.Image, 
            CreatedAt = h.CreatedAt
        }).ToList();
    }
}
