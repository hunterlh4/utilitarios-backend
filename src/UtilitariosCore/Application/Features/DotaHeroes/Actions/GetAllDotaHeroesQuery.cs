using MediatR;
using UtilitariosCore.Application.Features.DotaHeroes.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record GetAllDotaHeroesQuery : IRequest<Result<IEnumerable<DotaHeroDto>>>;

internal sealed class GetAllDotaHeroesQueryHandler(IDotaHeroRepository repository)
    : IRequestHandler<GetAllDotaHeroesQuery, Result<IEnumerable<DotaHeroDto>>>
{
    public async Task<Result<IEnumerable<DotaHeroDto>>> Handle(GetAllDotaHeroesQuery request, CancellationToken cancellationToken)
    {
        var heroes = await repository.GetAll();
        return heroes.Select(h => new DotaHeroDto
        {
            Id = h.Id, Name = h.Name, Image = h.Image, CreatedAt = h.CreatedAt
        }).ToList();
    }
}
