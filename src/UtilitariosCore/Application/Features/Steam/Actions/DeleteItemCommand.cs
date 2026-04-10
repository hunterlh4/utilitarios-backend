using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record DeleteItemCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSteamItemCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteItemCommand, Result>
{
    public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsItems(request.Id);
        if (!exists) return Errors.NotFound("Item no encontrado.");
        await repository.DeleteItems(request.Id);
        return Results.NoContent();
    }
}
