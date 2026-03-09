using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record DeleteActressCommand(int Id) : IRequest<Result>;

internal sealed class DeleteActressCommandHandler(
    IActressJavRepository actressRepository,
    IMediaRepository mediaRepository)
    : IRequestHandler<DeleteActressCommand, Result>
{
    public async Task<Result> Handle(DeleteActressCommand request, CancellationToken cancellationToken)
    {
        var actress = await actressRepository.GetActressJavById(request.Id);
        if (actress == null)
            return Results.NotFound("Actriz no encontrada.");

        // Eliminar imagen asociada si existe
        if (!string.IsNullOrEmpty(actress.Image))
        {
            var media = await mediaRepository.GetMediaByUrl(actress.Image);
            if (media != null)
                await mediaRepository.DeleteMedia(media.Id);
        }

        await actressRepository.DeleteActressJav(request.Id);
        return Results.NoContent();
    }
}
