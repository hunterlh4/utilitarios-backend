using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record UpdateProyectLinksCommand(
    int ProyectId,
    List<UpdateProyectLinkItem> Links
) : IRequest<Result>;

public record UpdateProyectLinkItem(int? Id, string Url, string? Name, int OrderIndex);

public class UpdateProyectLinksCommandValidator : AbstractValidator<UpdateProyectLinksCommand>
{
    public UpdateProyectLinksCommandValidator()
    {
        RuleFor(x => x.ProyectId).GreaterThan(0);
        RuleForEach(x => x.Links).ChildRules(link =>
        {
            link.RuleFor(l => l.Url).NotEmpty().MaximumLength(1000);
        });
    }
}

internal sealed class UpdateProyectLinksCommandHandler(
    IProyectRepository proyectRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<UpdateProyectLinksCommand, Result>
{
    public async Task<Result> Handle(UpdateProyectLinksCommand request, CancellationToken cancellationToken)
    {
        var exists = await proyectRepository.Exists(request.ProyectId);
        if (!exists) return Errors.NotFound("Proyecto no encontrado.");

        var existing = (await linkRepository.GetLinksByRefId(request.ProyectId, LinkType.Project)).ToList();

        var incomingIds = request.Links.Where(l => l.Id.HasValue).Select(l => l.Id!.Value).ToHashSet();
        var toDelete = existing.Where(l => !incomingIds.Contains(l.Id)).ToList();

        foreach (var link in toDelete)
            await linkRepository.DeleteLink(link.Id);

        foreach (var item in request.Links)
        {
            if (item.Id.HasValue)
            {
                var current = existing.FirstOrDefault(l => l.Id == item.Id.Value);
                if (current is not null)
                {
                    current.Url = item.Url;
                    current.Name = item.Name;
                    current.OrderIndex = item.OrderIndex;
                    await linkRepository.UpdateLink(current);
                }
            }
            else
            {
                await linkRepository.CreateLink(new Link
                {
                    Type = LinkType.Project,
                    RefId = request.ProyectId,
                    Url = item.Url,
                    Name = item.Name,
                    OrderIndex = item.OrderIndex,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        return Results.NoContent();
    }
}
