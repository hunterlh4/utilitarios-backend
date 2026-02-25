using MediatR;
using UtilitariosCore.Application.Features.Hentais.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public class GetAllHentaisQuery : IRequest<Result<IEnumerable<HentaiDto>>>
{
    internal sealed class Handler(IHentaiRepository hentaiRepository)
        : IRequestHandler<GetAllHentaisQuery, Result<IEnumerable<HentaiDto>>>
    {
        public async Task<Result<IEnumerable<HentaiDto>>> Handle(GetAllHentaisQuery request, CancellationToken cancellationToken)
        {
            var items = await hentaiRepository.GetAllHentaisWithTags();

            return items.Select(x => new HentaiDto
            {
                Id = x.Hentai.Id,
                ApiId = x.Hentai.ApiId,
                Title = x.Hentai.Title,
                Image = x.Hentai.Image,
                Episodes = x.Hentai.Episodes,
                Status = x.Hentai.Status,
                CreatedAt = x.Hentai.CreatedAt,
                Tags = x.Tags
            }).ToList();
        }
    }
}
