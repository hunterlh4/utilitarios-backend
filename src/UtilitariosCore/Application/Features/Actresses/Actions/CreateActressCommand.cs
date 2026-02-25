using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record CreateActressCommand : IRequest<Result<CreateActressJavDto>>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];

    public sealed class Validator : AbstractValidator<CreateActressCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IActressJavRepository actressRepository,
        ITagRepository tagRepository)
        : IRequestHandler<CreateActressCommand, Result<CreateActressJavDto>>
    {
        public async Task<Result<CreateActressJavDto>> Handle(CreateActressCommand request, CancellationToken cancellationToken)
        {
            var newActress = new ActressJav
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var actressId = await actressRepository.CreateActress(newActress);

            if (request.Tags.Count > 0)
                await tagRepository.ReplaceTagsForRefId(actressId, TagType.ActressJav, request.Tags);

            return Results.Created(new CreateActressJavDto { Id = actressId });
        }
    }
}
