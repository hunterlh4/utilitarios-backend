using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record UpdateAnimeGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<UpdateAnimeGaleryCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IGaleryRepository repository) 
        : IRequestHandler<UpdateAnimeGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetAnimeGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            item.Name = StringNormalizer.ToTitleCase(request.Name);
            await repository.UpdateAnimeGalery(item);

            return Results.NoContent();
        }
    }
}
