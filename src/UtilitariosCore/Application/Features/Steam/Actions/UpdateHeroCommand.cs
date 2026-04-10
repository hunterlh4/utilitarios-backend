using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record   UpdateHeroCommand (int Id) : IRequest<Result>
{
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
}

internal sealed class UpdateHeroCommandHandler(ISteamRepository repository)
    : IRequestHandler<UpdateHeroCommand, Result>
{
    public async Task<Result> Handle(UpdateHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = await repository.GetByIdHero(request.Id);
        if (hero is null) return Errors.NotFound("Héroe no encontrado.");
        hero.Name = request.Name;
        hero.Image = request.Image;
        await repository.UpdateHero(hero);
        return Results.NoContent();
    }
}
