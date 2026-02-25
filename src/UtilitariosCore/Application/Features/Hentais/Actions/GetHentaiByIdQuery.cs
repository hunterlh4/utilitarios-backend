using MediatR;
using UtilitariosCore.Application.Features.Hentais.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record GetHentaiByIdQuery(int Id) : IRequest<Result<HentaiDto>>
{
    internal sealed class Handler(IHentaiRepository hentaiRepository)
        : IRequestHandler<GetHentaiByIdQuery, Result<HentaiDto>>
    {
        public async Task<Result<HentaiDto>> Handle(GetHentaiByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await hentaiRepository.GetHentaiWithTagsById(request.Id);

            if (item is null) return Errors.NotFound();

            return new HentaiDto
            {
                Id = item.Hentai.Id,
                ApiId = item.Hentai.ApiId,
                Title = item.Hentai.Title,
                Image = item.Hentai.Image,
                Episodes = item.Hentai.Episodes,
                Status = item.Hentai.Status,
                CreatedAt = item.Hentai.CreatedAt,
                Tags = item.Tags
            };
        }
    }
}
