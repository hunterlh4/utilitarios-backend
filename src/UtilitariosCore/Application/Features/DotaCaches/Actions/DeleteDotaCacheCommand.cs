using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record DeleteDotaCacheCommand(int Id) : IRequest<Result>;

internal sealed class DeleteDotaCacheCommandHandler(IDotaCacheRepository repository)
    : IRequestHandler<DeleteDotaCacheCommand, Result>
{
    public async Task<Result> Handle(DeleteDotaCacheCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Cache no encontrado.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
