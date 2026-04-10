using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record DeleteTreasureCommand(int Id) : IRequest<Result>;

internal sealed class DeleteTreasureCommandHandler(ISteamRepository repository)
    : IRequestHandler<DeleteTreasureCommand, Result>
{
    public async Task<Result> Handle(DeleteTreasureCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsTreasure(request.Id);
        if (!exists) return Errors.NotFound("Cofre no encontrado.");
        await repository.DeleteTreasure(request.Id);
        return Results.NoContent();
    }
}
