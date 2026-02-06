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
            var item = await hentaiRepository.GetHentaiById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            return new HentaiDto
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
