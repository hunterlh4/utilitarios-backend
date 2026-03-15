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

            var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(request.Name);
            var canonicalForm = StringNormalizer.GetCanonicalFormForComparison(request.Name);
            var currentCanonicalForm = StringNormalizer.GetCanonicalFormForComparison(actress.Name);
            
            // Verificar si el nombre cambió y si ya existe otra actriz con ese nombre (detecta nombres invertidos)
            if (currentCanonicalForm != canonicalForm)
            {
                var exists = await actressAdultRepository.CheckActressNameExists(canonicalForm);
                if (exists)
                    return Errors.BadRequest($"Ya existe otra actriz con el nombre '{normalizedName}'.");
            }

            actress.Name = normalizedName;
            await actressAdultRepository.UpdateActressAdult(actress);

            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.ActressAdult, request.Tags);

            return Results.NoContent();
        }
    }
}
