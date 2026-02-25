using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record DeleteTagCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(ITagRepository tagRepository)
        : IRequestHandler<DeleteTagCommand, Result>
    {
        public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await tagRepository.GetTagById(request.Id);
            if (tag is null) return Errors.NotFound("Tag no encontrado.");

            var inUse = await tagRepository.IsTagInUse(request.Id);
            if (inUse) return Errors.BadRequest("No se puede eliminar el tag porque está en uso.");

            await tagRepository.DeleteTag(request.Id);
            return Results.NoContent();
        }
    }
}
