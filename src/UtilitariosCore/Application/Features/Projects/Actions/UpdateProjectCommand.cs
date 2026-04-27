using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public class UpdateProjectCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public List<int>? TagIds { get; set; }

    public sealed class Validator : AbstractValidator<UpdateProjectCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Url).MaximumLength(1000).When(x => x.Url is not null);
        }
    }

    internal sealed class Handler(
        IProjectRepository projectRepository,
        ITagRepository tagRepository)
        : IRequestHandler<UpdateProjectCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetById(request.Id);
            if (project is null) return Errors.NotFound("Proyecto no encontrado.");

            project.Name = request.Name;
            project.Description = request.Description;
            project.Url = request.Url;

            await projectRepository.Update(project);
            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.Project, request.TagIds ?? []);

            return Results.NoContent();
        }
    }
}
