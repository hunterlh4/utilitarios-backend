using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record UpdateAnimeGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;
    public int? MediaId { get; set; }

    public sealed class Validator : AbstractValidator<UpdateAnimeGaleryCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IAnimeGaleryRepository repository,
        IMediaRepository mediaRepository) 
        : IRequestHandler<UpdateAnimeGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetAnimeGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            item.Name = StringNormalizer.ToTitleCase(request.Name);
            await repository.UpdateAnimeGalery(item);

            // Si se proporciona MediaId, intercambiar posiciones
            if (request.MediaId.HasValue)
            {
                var targetMedia = await mediaRepository.GetMediaById(request.MediaId.Value);
                if (targetMedia is null || targetMedia.RefId != request.Id || targetMedia.Type != Domain.Enums.MediaType.AnimeGalery)
                {
                    return Errors.BadRequest("Media not found or doesn't belong to this galery");
                }

                // Obtener todos los medias de la galería ordenados
                var allMedias = (await mediaRepository.GetMediaByRefId(request.Id, Domain.Enums.MediaType.AnimeGalery))
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
