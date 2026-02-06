using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record UpdateJavCommand(int Id) : IRequest<Result>
{
    public string Code { get; set; } = string.Empty;
    public string ActressName { get; set; } = string.Empty;
    public string? ActressUrl { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();

    internal sealed class Handler(
        IJavRepository javRepository,
        IActressRepository actressRepository,
        ILinkRepository linkRepository) 
        : IRequestHandler<UpdateJavCommand, Result>
    {
        public async Task<Result> Handle(UpdateJavCommand request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return Errors.BadRequest("Code is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ActressName))
            {
                return Errors.BadRequest("ActressName is required.");
            }

            // Buscar o crear actriz
            var actress = await actressRepository.GetActressByName(request.ActressName);

            int actressId;

            if (actress == null)
            {
                // Crear nueva actriz
                var newActress = new Actress
                {
                    Name = request.ActressName,
                    CreatedAt = DateTime.UtcNow
                };

                actressId = await actressRepository.CreateActress(newActress);
            }
            else
            {
                actressId = actress.Id;
            }

            // Si tiene URL, verificar si ya existe ese link
            if (!string.IsNullOrWhiteSpace(request.ActressUrl))
            {
                var existingLinks = await linkRepository.GetLinksByRefId(actressId, LinkType.Actress);
                var linkExists = existingLinks.Any(l => l.Url == request.ActressUrl);

                // Solo crear el link si no existe
                if (!linkExists)
                {
                    var actressLink = new Link
                    {
                        Type = LinkType.Actress,
                        RefId = actressId,
                        Name = null,
                        Url = request.ActressUrl,
                        CreatedAt = DateTime.UtcNow
                    };
                    await linkRepository.CreateLink(actressLink);
                }
            }

            // Actualizar Jav
            item.Code = request.Code.ToUpper();
            item.ActressId = actressId;
            item.Image = request.Image;

            await javRepository.UpdateJav(item);

            // Actualizar links del Jav - eliminar los existentes y crear los nuevos
            await linkRepository.DeleteLinksByRefId(request.Id, LinkType.Jav);

            if (request.Links != null && request.Links.Any())
            {
                foreach (var url in request.Links)
                {
                    var link = new Link
                    {
                        Type = LinkType.Jav,
                        RefId = request.Id,
                        Name = null,
                        Url = url,
                        CreatedAt = DateTime.UtcNow
                    };
                    await linkRepository.CreateLink(link);
                }
            }

            return Results.NoContent();
        }
    }
}
