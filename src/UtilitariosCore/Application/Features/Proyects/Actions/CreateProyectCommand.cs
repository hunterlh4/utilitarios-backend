using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record CreateProyectCommand(
    string Name,
    string? Description,
    string? Url,
    List<int>? TagIds,
    List<string>? Links
) : IRequest<Result<int>>;

public class CreateProyectCommandValidator : AbstractValidator<CreateProyectCommand>
{
    public CreateProyectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Url).MaximumLength(1000).When(x => x.Url is not null);
        RuleForEach(x => x.Links).NotEmpty().When(x => x.Links is not null);
    }
}

internal sealed class CreateProyectCommandHandler(
    IProyectRepository proyectRepository,
    ITagRepository tagRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<CreateProyectCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateProyectCommand request, CancellationToken cancellationToken)
    {
        var proyect = new Proyect
        {
            Name = request.Name,
            Description = request.Description,
            Url = request.Url,
            CreatedAt = DateTime.Now
        };

        int id = await proyectRepository.Create(proyect);

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

        return id;
    }
}
