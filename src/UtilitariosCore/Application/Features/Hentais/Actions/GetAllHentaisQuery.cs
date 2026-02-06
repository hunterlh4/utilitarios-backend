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
            var items = await hentaiRepository.GetAllHentais();

            return items.Select(x => new HentaiDto
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
