using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record UpdateAnimeStatusCommand : IRequest<Result>
{
    public int Id { get; set; }
    public ContentStatus Status { get; set; }

    public sealed class Validator : AbstractValidator<UpdateAnimeStatusCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("El estado no es válido.");
        }
    }

    internal sealed class Handler(IAnimeRepository animeRepository)
        : IRequestHandler<UpdateAnimeStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeStatusCommand request, CancellationToken cancellationToken)
        {
            var item = await animeRepository.GetAnimeById(request.Id);
            if (item is null) return Errors.NotFound("Anime no encontrado.");

            item.Status = request.Status;
            await animeRepository.UpdateAnime(item);

            return Results.NoContent();
        }
    }
}