using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record UpdateDotaHeroCommand(int Id, string Name, string? Image) : IRequest<Result>;

internal sealed class UpdateDotaHeroCommandHandler(IDotaHeroRepository repository)
    : IRequestHandler<UpdateDotaHeroCommand, Result>
{
    public async Task<Result> Handle(UpdateDotaHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = await repository.GetById(request.Id);
        if (hero is null) return Errors.NotFound("Héroe no encontrado.");
        hero.Name = request.Name;
        hero.Image = request.Image;
        await repository.Update(hero);
        return Results.NoContent();
    }
}
