using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record DeleteHeroCommand(int Id) : IRequest<Result>;

internal sealed class DeleteHeroCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteHeroCommand, Result>
{
    public async Task<Result> Handle(DeleteHeroCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsHero(request.Id);
        if (!exists) return Errors.NotFound("Héroe no encontrado.");
        await repository.DeleteHero(request.Id);
        return Results.NoContent();
    }
}
