using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record DeleteProyectCommand(int Id) : IRequest<Result>;

internal sealed class DeleteProyectCommandHandler(
    IProyectRepository proyectRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<DeleteProyectCommand, Result>
{
    public async Task<Result> Handle(DeleteProyectCommand request, CancellationToken cancellationToken)
    {
        var exists = await proyectRepository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Proyecto no encontrado.");

        await linkRepository.DeleteLinksByRefId(request.Id, LinkType.Project);
        await proyectRepository.Delete(request.Id);

        return Results.NoContent();
    }
}
