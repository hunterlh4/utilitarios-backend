using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record DeleteItemDropCommand(int Id) : IRequest<Result>;

internal sealed class DeleteItemDropCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteItemDropCommand, Result>
{
    public async Task<Result> Handle(DeleteItemDropCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsDrops(request.Id);
        if (!exists) return Errors.NotFound("Drop no encontrado.");
        await repository.DeleteDrops(request.Id);
        return Results.NoContent();
    }
}
