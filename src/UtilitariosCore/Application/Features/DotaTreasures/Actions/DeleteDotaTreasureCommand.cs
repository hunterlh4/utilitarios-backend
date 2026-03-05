using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record DeleteDotaTreasureCommand(int Id) : IRequest<Result>;

internal sealed class DeleteDotaTreasureCommandHandler(IDotaTreasureRepository repository)
    : IRequestHandler<DeleteDotaTreasureCommand, Result>
{
    public async Task<Result> Handle(DeleteDotaTreasureCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Cofre no encontrado.");
        await repository.Delete(request.Id);
        return Results.NoContent();
    }
}
