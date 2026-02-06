using MediatR;
using UtilitariosCore.Application.Features.Animes.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record GetAnimeByIdQuery(int Id) : IRequest<Result<AnimeDto>>
{
    internal sealed class Handler(IAnimeRepository animeRepository) : IRequestHandler<GetAnimeByIdQuery, Result<AnimeDto>>
    {
        public async Task<Result<AnimeDto>> Handle(GetAnimeByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await animeRepository.GetAnimeById(request.Id);

            if (item == null)
            {
                return Errors.NotFound();
            }

            return new AnimeDto
            {
                Id = item.Id,
                ApiId = item.ApiId,
                Title = item.Title,
                Image = item.Image,
                Episodes = item.Episodes,
                Status = item.Status,
                CreatedAt = item.CreatedAt
            };
        }
    }
}
