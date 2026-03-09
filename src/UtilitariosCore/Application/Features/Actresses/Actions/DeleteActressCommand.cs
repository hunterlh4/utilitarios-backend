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
            return Errors.NotFound("Actriz no encontrada.");

        // Eliminar medias asociados si existen
        var medias = await mediaRepository.GetMediaByRefId(request.Id, Domain.Enums.MediaType.ActressJav);
        foreach (var media in medias)
        {
            await mediaRepository.DeleteMedia(media.Id);
        }

        await actressRepository.DeleteActressJav(request.Id);
        return Results.NoContent();
    }
}
