using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class BulkActressInput
{
    public string Name { get; set; } = string.Empty;
    public string? Url { get; set; }
}

public class BulkCreateJavCommand : IRequest<Result<CreateJavDto>>
{
    public string Code { get; set; } = string.Empty;
    public List<BulkActressInput> Actresses { get; set; } = new();
    public string? Image { get; set; }
    public List<string> Links { get; set; } = new();

    public sealed class Validator : AbstractValidator<BulkCreateJavCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("El código es requerido.");
            RuleForEach(x => x.Actresses).ChildRules(a =>
            {
                a.RuleFor(x => x.Url).Must(url => url == null || Uri.TryCreate(url, UriKind.Absolute, out _))
                    .WithMessage("La URL de la actriz no es válida.");
            });
            RuleForEach(x => x.Links).NotEmpty().WithMessage("El link no puede estar vacío.");
        }
    }

    internal sealed class Handler(
        IJavRepository javRepository,
        IActressJavRepository actressJavRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<BulkCreateJavCommand, Result<CreateJavDto>>
    {
        public async Task<Result<CreateJavDto>> Handle(BulkCreateJavCommand request, CancellationToken cancellationToken)
        {
            // Si no viene ninguna actriz, usar "Sin Nombre" por defecto
            if (request.Actresses == null || request.Actresses.Count == 0)
            {
                request.Actresses = [new BulkActressInput { Name = "Sin Nombre" }];
            }

            foreach (var actress in request.Actresses.Where(a => string.IsNullOrWhiteSpace(a.Name)))
            {
                actress.Name = "Sin Nombre";
            }

            // Verificar si el Jav ya existe por Code
            var existingJav = await javRepository.GetJavByCode(request.Code);
            if (existingJav != null)
            {
                return Results.Created(new CreateJavDto { Id = existingJav.Id });
            }

            try
            {
                // Crear Jav
                var newJav = new Jav
                {
                    Code = request.Code.ToUpper(),
                    Image = request.Image ?? string.Empty,
                    Status = ContentStatus.Proximamente,
                    CreatedAt = DateTime.UtcNow
                };

                var javId = await javRepository.CreateJav(newJav);

                // Procesar cada actriz
                foreach (var actressInput in request.Actresses)
                {
                    var actress = await actressJavRepository.GetActressByName(actressInput.Name);

                    int actressId;

                    if (actress == null)
                    {
                        var newActress = new ActressJav
                        {
                            Name = actressInput.Name,
                            CreatedAt = DateTime.UtcNow
                        };
                        actressId = await actressJavRepository.CreateActress(newActress);
                    }
                    else
                    {
                        actressId = actress.Id;
                    }

                    await javRepository.AddActressToJav(javId, actressId);

                    if (!string.IsNullOrWhiteSpace(actressInput.Url))
                    {
                        var existingLinks = await linkRepository.GetLinksByRefId(actressId, LinkType.ActressJav);
                        if (!existingLinks.Any(l => l.Url == actressInput.Url))
                        {
                            await linkRepository.CreateLink(new Link
                            {
                                Type = LinkType.ActressJav,
                                RefId = actressId,
                                Name = null,
                                Url = actressInput.Url,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                // Crear links del Jav
                if (request.Links != null && request.Links.Count > 0)
                {
                    foreach (var url in request.Links)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            await linkRepository.CreateLink(new Link
                            {
                                Type = LinkType.Jav,
                                RefId = javId,
                                Name = null,
                                Url = url,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                return Results.Created(new CreateJavDto { Id = javId });
            }
            catch (Exception ex)
            {
                return Errors.BadRequest($"Error creating Jav {request.Code}: {ex.Message}");
            }
        }
    }
}
