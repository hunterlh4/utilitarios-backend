using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record DeleteAnimeCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAnimeRepository animeRepository) 
        : IRequestHandler<DeleteAnimeCommand, Result>
    {
        public async Task<Result> Handle(DeleteAnimeCommand request, CancellationToken cancellationToken)
        {
            var item = await animeRepository.GetAnimeById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            await animeRepository.DeleteAnime(request.Id);

            return Results.NoContent();
        }
    }
}
