using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record UpdateGirlGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;
    public int? MediaId { get; set; }

    internal sealed class Handler(
        IGirlGaleryRepository repository,
        IMediaRepository mediaRepository) 
        : IRequestHandler<UpdateGirlGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateGirlGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetGirlGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            item.Name = request.Name;
            await repository.UpdateGirlGalery(item);

            // Si se proporciona MediaId, intercambiar posiciones
            if (request.MediaId.HasValue)
            {
                var targetMedia = await mediaRepository.GetMediaById(request.MediaId.Value);
                if (targetMedia is null || targetMedia.RefId != request.Id || targetMedia.Type != Domain.Enums.MediaType.GirlGalery)
                {
                    return Errors.BadRequest("Media not found or doesn't belong to this galery");
                }

                // Obtener todos los medias de la galería ordenados
                var allMedias = (await mediaRepository.GetMediaByRefId(request.Id, Domain.Enums.MediaType.GirlGalery))
                    .OrderBy(m => m.OrderIndex)
                    .ToList();

                // Encontrar la posición actual del media objetivo
                var targetIndex = allMedias.FindIndex(m => m.Id == request.MediaId.Value);
                if (targetIndex > 0) // Solo intercambiar si no está en la primera posición
                {
                    var firstMedia = allMedias[0];
                    var targetMediaPosition = targetMedia.OrderIndex;
                    var firstMediaPosition = firstMedia.OrderIndex;

                    // Intercambiar las posiciones
                    await mediaRepository.UpdateMediaOrder(targetMedia.Id, firstMediaPosition);
                    await mediaRepository.UpdateMediaOrder(firstMedia.Id, targetMediaPosition);
                }
            }

            return Results.NoContent();
        }
    }
}
