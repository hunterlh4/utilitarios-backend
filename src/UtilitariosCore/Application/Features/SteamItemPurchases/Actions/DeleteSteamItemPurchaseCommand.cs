using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record DeleteSteamItemPurchaseCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSteamItemPurchaseCommandHandler(ISteamItemPurchaseRepository repository)
    : IRequestHandler<DeleteSteamItemPurchaseCommand, Result>
{
    public async Task<Result> Handle(DeleteSteamItemPurchaseCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Compra no encontrada.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
