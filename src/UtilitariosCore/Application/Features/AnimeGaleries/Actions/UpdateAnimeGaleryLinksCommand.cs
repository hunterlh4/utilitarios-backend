using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record UpdateAnimeGaleryLinksCommand(int Id, List<string> Links) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateAnimeGaleryLinksCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleForEach(x => x.Links).NotEmpty().WithMessage("La URL del link no puede estar vacía.");
        }
    }

    internal sealed class Handler(
        IAnimeGaleryRepository repository,
        ILinkRepository linkRepository)
        : IRequestHandler<UpdateAnimeGaleryLinksCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeGaleryLinksCommand request, CancellationToken cancellationToken)
        {
            var galery = await repository.GetAnimeGaleryById(request.Id);
            if (galery is null) return Errors.NotFound("Galería no encontrada.");

            var existingLinks = await linkRepository.GetLinksByRefId(request.Id, LinkType.AnimeGalery);
            var existingByUrl = existingLinks.ToDictionary(l => l.Url, l => l);
            var incomingUrls = request.Links.ToHashSet();

            foreach (var link in existingByUrl.Values.Where(l => !incomingUrls.Contains(l.Url)))
                await linkRepository.DeleteLink(link.Id);

            for (int i = 0; i < request.Links.Count; i++)
            {
                var url = request.Links[i];
                var orderIndex = i + 1;

                if (existingByUrl.TryGetValue(url, out var existing))
                {
                    existing.OrderIndex = orderIndex;
                    await linkRepository.UpdateLink(existing);
                }
                else
                {
                    await linkRepository.CreateLink(new Link
                    {
                        Type = LinkType.AnimeGalery,
                        RefId = request.Id,
                        Url = url,
                        OrderIndex = orderIndex
                    });
                }
            }

            return Results.NoContent();
        }
    }
}
