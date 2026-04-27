using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public record UpdateProjectLinkItem(int? Id, string Url, string? Name, int OrderIndex);

public class UpdateProjectLinksCommand : IRequest<Result>
{
    public int ProjectId { get; set; }
    public List<UpdateProjectLinkItem> Links { get; set; } = [];

    public sealed class Validator : AbstractValidator<UpdateProjectLinksCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId).GreaterThan(0);
            RuleForEach(x => x.Links).ChildRules(link =>
            {
                link.RuleFor(l => l.Url).NotEmpty().MaximumLength(1000);
            });
        }
    }

    internal sealed class Handler(
        IProjectRepository projectRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<UpdateProjectLinksCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectLinksCommand request, CancellationToken cancellationToken)
        {
            var exists = await projectRepository.Exists(request.ProjectId);
            if (!exists) return Errors.NotFound("Proyecto no encontrado.");

            var existing = (await linkRepository.GetLinksByRefId(request.ProjectId, LinkType.Project)).ToList();
            var incomingIds = request.Links.Where(l => l.Id.HasValue).Select(l => l.Id!.Value).ToHashSet();

            foreach (var link in existing.Where(l => !incomingIds.Contains(l.Id)))
                await linkRepository.DeleteLink(link.Id);

            foreach (var item in request.Links)
            {
                if (item.Id.HasValue)
                {
                    var current = existing.FirstOrDefault(l => l.Id == item.Id.Value);
                    if (current is not null)
                    {
                        current.Url = item.Url;
                        current.Name = item.Name ?? string.Empty;
                        current.OrderIndex = item.OrderIndex;
                        await linkRepository.UpdateLink(current);
                    }
                }
                else
                {
                    await linkRepository.CreateLink(new Link
                    {
                        Type = LinkType.Project,
                        RefId = request.ProjectId,
                        Url = item.Url,
                        Name = item.Name ?? string.Empty,
                        OrderIndex = item.OrderIndex,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            return Results.NoContent();
        }
    }
}
