using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class BulkCreateJavCommand : IRequest<Result<CreateJavDto>>
{
    public string Code { get; set; } = string.Empty;
    public string ActressName { get; set; } = string.Empty;
    public string? ActressUrl { get; set; }
    public string? Image { get; set; }
    public List<string> Links { get; set; } = new();

    internal sealed class Handler(
        IJavRepository javRepository,
        IActressJavRepository actressJavRepository,
        ILinkRepository linkRepository) 
        : IRequestHandler<BulkCreateJavCommand, Result<CreateJavDto>>
    {
        public async Task<Result<CreateJavDto>> Handle(BulkCreateJavCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return Errors.BadRequest("Code is required.");
            }

            // Si no tiene nombre de actriz, usar "Sin Nombre"
            if (string.IsNullOrWhiteSpace(request.ActressName))
            {
                request.ActressName = "Sin Nombre";
            }

            // Verificar si el Jav ya existe por Code
            var existingJav = await javRepository.GetJavByCode(request.Code);

            if (existingJav != null)
            {
                // Si ya existe, devolver el ID del existente
                return Results.Created(new CreateJavDto
                {
                    Id = existingJav.Id
                });
            }

            try
            {
                // Buscar o crear actriz
                var actress = await actressJavRepository.GetActressByName(request.ActressName);

                int actressId;

                if (actress == null)
                {
                    var newActress = new ActressJav
                    {
                        Name = request.ActressName,
                        CreatedAt = DateTime.UtcNow
                    };

                    actressId = await actressJavRepository.CreateActress(newActress);
                }
                else
                {
                    actressId = actress.Id;
                }

                // Si tiene URL, verificar si ya existe ese link
                if (!string.IsNullOrWhiteSpace(request.ActressUrl))
                {
                    var existingLinks = await linkRepository.GetLinksByRefId(actressId, LinkType.ActressJav);
                    var linkExists = existingLinks.Any(l => l.Url == request.ActressUrl);

                    if (!linkExists)
                    {
                        var actressLink = new Link
                        {
                            Type = LinkType.ActressJav,
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
                    Image = request.Image ?? string.Empty,
                    Status = ContentStatus.Proximamente,
                    CreatedAt = DateTime.UtcNow
                };

                var javId = await javRepository.CreateJav(newJav);

                // Crear links del Jav
                if (request.Links != null && request.Links.Any())
                {
                    foreach (var url in request.Links)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
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
            catch (Exception ex)
            {
                return Errors.BadRequest($"Error creating Jav {request.Code}: {ex.Message}");
            }
        }
    }
}
