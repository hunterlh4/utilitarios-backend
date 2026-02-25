using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record CreateTagCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public TagType Type { get; set; }

    public sealed class Validator : AbstractValidator<CreateTagCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(x => x.Type).IsInEnum().WithMessage("El tipo no es válido.");
        }
    }

    internal sealed class Handler(ITagRepository tagRepository)
        : IRequestHandler<CreateTagCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag
            {
                Name = StringNormalizer.ToNormalizedTag(request.Name),
                Type = request.Type
            };

            var id = await tagRepository.CreateTag(tag);
            return Results.Created(id);
        }
    }
}
