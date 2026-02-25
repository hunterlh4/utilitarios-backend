using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record UpdateActressAdultCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<int> Tags { get; set; } = [];

    public sealed class Validator : AbstractValidator<UpdateActressAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IActressAdultRepository actressAdultRepository,
        ITagRepository tagRepository)
        : IRequestHandler<UpdateActressAdultCommand, Result>
    {
        public async Task<Result> Handle(UpdateActressAdultCommand request, CancellationToken cancellationToken)
        {
            var actress = await actressAdultRepository.GetActressAdultById(request.Id);
            if (actress == null) return Errors.NotFound("Actriz no encontrada.");

            actress.Name = StringNormalizer.ToTitleCase(request.Name);
            await actressAdultRepository.UpdateActressAdult(actress);

            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.ActressAdult, request.Tags);

            return Results.NoContent();
        }
    }
}
