using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record DeleteDotaHeroCommand(int Id) : IRequest<Result>;

internal sealed class DeleteDotaHeroCommandHandler(IDotaHeroRepository repository)
    : IRequestHandler<DeleteDotaHeroCommand, Result>
{
    public async Task<Result> Handle(DeleteDotaHeroCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Héroe no encontrado.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
