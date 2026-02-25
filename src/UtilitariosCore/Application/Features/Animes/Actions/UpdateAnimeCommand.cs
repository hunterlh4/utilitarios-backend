using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record UpdateAnimeCommand(int Id) : IRequest<Result>
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }

    public sealed class Validator : AbstractValidator<UpdateAnimeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("El título es requerido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.Episodes).GreaterThan(0).WithMessage("Los episodios deben ser mayor a 0.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("El estado no es válido.");
        }
    }

    internal sealed class Handler(IAnimeRepository animeRepository) 
        : IRequestHandler<UpdateAnimeCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeCommand request, CancellationToken cancellationToken)
        {
            var item = await animeRepository.GetAnimeById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            item.Title = StringNormalizer.ToTitleCase(request.Title);
            item.Image = request.Image;
            item.Episodes = request.Episodes;
            item.Status = request.Status;

            await animeRepository.UpdateAnime(item);

            return Results.NoContent();
        }
    }
}
