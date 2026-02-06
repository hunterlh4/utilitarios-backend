using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class CreateJavCommand : IRequest<Result<CreateJavDto>>
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
        : IRequestHandler<CreateJavCommand, Result<CreateJavDto>>
    {
        public async Task<Result<CreateJavDto>> Handle(CreateJavCommand request, CancellationToken cancellationToken)
        {
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

            // Crear Jav
            var newJav = new Jav
            {
                Code = request.Code.ToUpper(),
                ActressId = actressId,
                Image = request.Image,
                Status = ContentStatus.Proximamente,
                CreatedAt = DateTime.UtcNow
            };

            var javId = await javRepository.CreateJav(newJav);

            // Crear links del Jav
            if (request.Links != null && request.Links.Any())
            {
                // Obtener links existentes para verificar duplicados
                var existingLinks = await linkRepository.GetLinksByRefId(javId, LinkType.Jav);
                var existingUrls = existingLinks.Select(l => l.Url).ToHashSet();

                foreach (var url in request.Links)
                {
                    // Solo crear el link si no existe
                    if (!existingUrls.Contains(url))
                    {
                        var link = new Link
                        {
                            Type = LinkType.Jav,
                            RefId = javId,
                            Name = null,
                            Url = url,
                            CreatedAt = DateTime.UtcNow
                        };
                        await linkRepository.CreateLink(link);
                    }
                }
            }

            return Results.Created(new CreateJavDto
            {
                Id = javId
            });
        }
    }
}
