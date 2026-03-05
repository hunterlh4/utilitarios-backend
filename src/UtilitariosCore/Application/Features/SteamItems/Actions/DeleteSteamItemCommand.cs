using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record DeleteSteamItemCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSteamItemCommandHandler(ISteamItemRepository repository)
    : IRequestHandler<DeleteSteamItemCommand, Result>
{
    public async Task<Result> Handle(DeleteSteamItemCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Item no encontrado.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
