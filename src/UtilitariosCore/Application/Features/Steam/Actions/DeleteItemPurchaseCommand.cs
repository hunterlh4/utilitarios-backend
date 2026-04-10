using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record DeleteItemPurchaseCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSteamItemPurchaseCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteItemPurchaseCommand, Result>
{
    public async Task<Result> Handle(DeleteItemPurchaseCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsPurchase(request.Id);
        if (!exists) return Errors.NotFound("Compra no encontrada.");
        await repository.DeletePurchase(request.Id);
        return Results.NoContent();
    }
}
