using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record UpdateJavCommand(int Id) : IRequest<Result>
{
    public string Code { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = new();
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();

    public sealed class Validator : AbstractValidator<UpdateJavCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Code).NotEmpty().WithMessage("El código es requerido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.ActressIds).NotEmpty().WithMessage("Se requiere al menos una actriz.");
            RuleForEach(x => x.ActressIds).GreaterThan(0).WithMessage("El ID de la actriz debe ser mayor a 0.");
            RuleForEach(x => x.Links).NotEmpty().WithMessage("El link no puede estar vacío.");
        }
    }

    internal sealed class Handler(
        IJavRepository javRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<UpdateJavCommand, Result>
    {
        public async Task<Result> Handle(UpdateJavCommand request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavById(request.Id);
            if (item is null) return Errors.NotFound();

            item.Code = request.Code.ToUpper();
            item.Image = request.Image;
            await javRepository.UpdateJav(item);

            // Diff actrices: eliminar las que ya no vienen, agregar las nuevas
            var currentIds = (await javRepository.GetActressIdsByJavId(request.Id)).ToHashSet();
            var incomingIds = request.ActressIds.ToHashSet();

            foreach (var id in currentIds.Except(incomingIds))
                await javRepository.RemoveActressFromJav(request.Id, id);

            foreach (var id in incomingIds.Except(currentIds))
                await javRepository.AddActressToJav(request.Id, id);

            // Diff links: eliminar los que ya no vienen, agregar los nuevos
            var currentLinks = (await linkRepository.GetLinksByRefId(request.Id, LinkType.Jav)).ToList();
            var incomingUrls = request.Links
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var link in currentLinks.Where(l => !incomingUrls.Contains(l.Url)))
                await linkRepository.DeleteLink(link.Id);

            var existingUrls = currentLinks.Select(l => l.Url).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var url in incomingUrls.Where(u => !existingUrls.Contains(u)))
            {
                await linkRepository.CreateLink(new Link
                {
                    Type = LinkType.Jav,
                    RefId = request.Id,
                    Name = null,
                    Url = url,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return Results.NoContent();
        }
    }
}
