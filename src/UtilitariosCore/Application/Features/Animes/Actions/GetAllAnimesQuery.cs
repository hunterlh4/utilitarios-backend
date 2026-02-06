using MediatR;
using UtilitariosCore.Application.Features.Animes.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public class GetAllAnimesQuery : IRequest<Result<IEnumerable<AnimeDto>>>
{
    internal sealed class Handler(IAnimeRepository animeRepository) : IRequestHandler<GetAllAnimesQuery, Result<IEnumerable<AnimeDto>>>
    {
        public async Task<Result<IEnumerable<AnimeDto>>> Handle(GetAllAnimesQuery request, CancellationToken cancellationToken)
        {
            var items = await animeRepository.GetAllAnimes();

            return items.Select(x => new AnimeDto
            {
                Id = x.Id,
                ApiId = x.ApiId,
                Title = x.Title,
                Image = x.Image,
                Episodes = x.Episodes,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
