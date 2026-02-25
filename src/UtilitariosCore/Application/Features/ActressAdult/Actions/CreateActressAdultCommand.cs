using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record CreateActressAdultCommand : IRequest<Result<CreateActressAdultDto>>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];

    public sealed class Validator : AbstractValidator<CreateActressAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerida.");
        }
    }

    internal sealed class Handler(
        IActressAdultRepository actressAdultRepository,
        ITagRepository tagRepository)
        : IRequestHandler<CreateActressAdultCommand, Result<CreateActressAdultDto>>
    {
        public async Task<Result<CreateActressAdultDto>> Handle(CreateActressAdultCommand request, CancellationToken cancellationToken)
        {
            var newActress = new ActressAdult
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var actressId = await actressAdultRepository.CreateActressAdult(newActress);

            if (request.Tags.Count > 0)
                await tagRepository.ReplaceTagsForRefId(actressId, TagType.ActressAdult, request.Tags);

            return Results.Created(new CreateActressAdultDto { Id = actressId });
        }
    }
}
