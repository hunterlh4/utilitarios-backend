using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record UpdateAnimeCommand(int Id) : IRequest<Result>
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public Domain.Enums.ContentStatus Status { get; set; }

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

            item.Title = request.Title;
            item.Image = request.Image;
            item.Episodes = request.Episodes;
            item.Status = request.Status;

            await animeRepository.UpdateAnime(item);

            return Results.NoContent();
        }
    }
}
