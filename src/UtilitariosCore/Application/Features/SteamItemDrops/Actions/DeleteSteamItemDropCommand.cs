using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record DeleteSteamItemDropCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSteamItemDropCommandHandler(ISteamItemDropRepository repository)
    : IRequestHandler<DeleteSteamItemDropCommand, Result>
{
    public async Task<Result> Handle(DeleteSteamItemDropCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Drop no encontrado.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
