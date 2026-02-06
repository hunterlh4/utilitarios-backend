using MediatR;
using UtilitariosCore.Application.Features.Shared.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record ReorderMediaCommand(int GaleryId, List<MediaOrderItem> Items) : IRequest<Result>;

internal sealed class ReorderMediaCommandHandler(
    IAnimeGaleryRepository galeryRepository,
    IMediaRepository mediaRepository) 
    : IRequestHandler<ReorderMediaCommand, Result>
{
    public async Task<Result> Handle(ReorderMediaCommand request, CancellationToken cancellationToken)
    {
        var galery = await galeryRepository.GetAnimeGaleryById(request.GaleryId);
        if (galery is null)
        {
            return Errors.NotFound("Galery not found");
        }

        if (request.Items == null || !request.Items.Any())
        {
            return Errors.BadRequest("Items list is required");
        }

        // Validar que todos los media pertenecen a esta galería
        foreach (var item in request.Items)
        {
            var media = await mediaRepository.GetMediaById(item.MediaId);
            if (media is null || media.RefId != request.GaleryId || media.Type != MediaType.AnimeGalery)
            {
                return Errors.BadRequest($"Media {item.MediaId} not found or doesn't belong to this galery");
            }
        }

        // Actualizar las posiciones
        foreach (var item in request.Items)
        {
            await mediaRepository.UpdateMediaOrder(item.MediaId, item.OrderIndex);
        }

        return Results.NoContent();
    }
}
