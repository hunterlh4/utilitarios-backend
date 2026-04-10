using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record DeleteCacheCommand(int Id) : IRequest<Result>;

internal sealed class DeleteCacheCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteCacheCommand, Result>
{
    public async Task<Result> Handle(DeleteCacheCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsCache(request.Id);
        if (!exists) return Errors.NotFound("Cache no encontrado.");
        await repository.DeleteCache(request.Id);
        return Results.NoContent();
    }
}
