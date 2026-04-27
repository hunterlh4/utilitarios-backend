using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public class CreateProjectCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public List<int>? TagIds { get; set; }
    public List<string>? Links { get; set; }

    public sealed class Validator : AbstractValidator<CreateProjectCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Url).MaximumLength(1000).When(x => x.Url is not null);
            RuleForEach(x => x.Links).NotEmpty().When(x => x.Links is not null);
        }
    }

    internal sealed class Handler(
        IProjectRepository projectRepository,
        ITagRepository tagRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<CreateProjectCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                Url = request.Url,
                CreatedAt = DateTime.UtcNow
            };

            int id = await projectRepository.Create(project);

            if (request.TagIds is { Count: > 0 })
                await tagRepository.ReplaceTagsForRefId(id, TagType.Project, request.TagIds);

            if (request.Links is { Count: > 0 })
            {
                for (int i = 0; i < request.Links.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(request.Links[i]))
                        await linkRepository.CreateLink(new Link
                        {
                            Type = LinkType.Project,
                            RefId = id,
                            Url = request.Links[i],
                            OrderIndex = i + 1,
                            CreatedAt = DateTime.UtcNow
                        });
                }
            }

            return Results.Created(id);
        }
    }
}
