using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record UpdateActressCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<int> TagIds { get; set; } = [];

    public sealed class Validator : AbstractValidator<UpdateActressCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IActressJavRepository actressRepository,
        ITagRepository tagRepository)
        : IRequestHandler<UpdateActressCommand, Result>
    {
        public async Task<Result> Handle(UpdateActressCommand request, CancellationToken cancellationToken)
        {
            var actress = await actressRepository.GetActressJavById(request.Id);
            if (actress == null) return Errors.NotFound("Actriz no encontrada.");

            var normalizedName = StringNormalizer.ToTitleCase(request.Name);
            var canonicalForm = StringNormalizer.GetCanonicalFormForComparison(request.Name);
            var currentCanonicalForm = StringNormalizer.GetCanonicalFormForComparison(actress.Name);
            
            // Verificar si el nombre cambió y si ya existe otra actriz con ese nombre (detecta nombres invertidos)
            if (currentCanonicalForm != canonicalForm)
            {
                var exists = await actressRepository.CheckActressNameExists(canonicalForm);
                if (exists)
                    return Errors.BadRequest($"Ya existe otra actriz con el nombre '{normalizedName}'.");
            }

            actress.Name = normalizedName;
            await actressRepository.UpdateActressJav(actress);

            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.ActressJav, request.TagIds);

            return Results.NoContent();
        }
    }
}
