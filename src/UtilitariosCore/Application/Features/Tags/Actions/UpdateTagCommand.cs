using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record UpdateTagCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<UpdateTagCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(ITagRepository tagRepository)
        : IRequestHandler<UpdateTagCommand, Result>
    {
        public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await tagRepository.GetTagById(request.Id);
            if (tag is null) return Errors.NotFound("Tag no encontrado.");

            tag.Name = StringNormalizer.ToNormalizedTag(request.Name);
            await tagRepository.UpdateTag(tag);

            return Results.NoContent();
        }
    }
}
