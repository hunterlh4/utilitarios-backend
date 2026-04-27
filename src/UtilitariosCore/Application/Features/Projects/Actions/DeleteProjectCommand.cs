using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public record DeleteProjectCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(
        IProjectRepository projectRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<DeleteProjectCommand, Result>
    {
        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var exists = await projectRepository.Exists(request.Id);
            if (!exists) return Errors.NotFound("Proyecto no encontrado.");

            await linkRepository.DeleteLinksByRefId(request.Id, LinkType.Project);
            await projectRepository.Delete(request.Id);

            return Results.NoContent();
        }
    }
}
