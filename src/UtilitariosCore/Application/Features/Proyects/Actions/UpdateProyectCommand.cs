using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record UpdateProyectCommand(
    int Id,
    string Name,
    string? Description,
    string? Url,
    List<int>? TagIds
) : IRequest<Result>;

public class UpdateProyectCommandValidator : AbstractValidator<UpdateProyectCommand>
{
    public UpdateProyectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Url).MaximumLength(1000).When(x => x.Url is not null);
    }
}

internal sealed class UpdateProyectCommandHandler(
    IProyectRepository proyectRepository,
    ITagRepository tagRepository)
    : IRequestHandler<UpdateProyectCommand, Result>
{
    public async Task<Result> Handle(UpdateProyectCommand request, CancellationToken cancellationToken)
    {
        var proyect = await proyectRepository.GetById(request.Id);
        if (proyect is null) return Errors.NotFound("Proyecto no encontrado.");

        proyect.Name = request.Name;
        proyect.Description = request.Description;
        proyect.Url = request.Url;

        await proyectRepository.Update(proyect);

        await tagRepository.ReplaceTagsForRefId(request.Id, TagType.Project, request.TagIds ?? []);

        return Results.NoContent();
    }
}
