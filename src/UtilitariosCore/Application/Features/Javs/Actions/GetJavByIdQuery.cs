using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record GetJavByIdQuery(int Id) : IRequest<Result<JavDto>>
{
    internal sealed class Handler(IJavRepository javRepository) 
        : IRequestHandler<GetJavByIdQuery, Result<JavDto>>
    {
        public async Task<Result<JavDto>> Handle(GetJavByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavWithDetailsById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            return new JavDto
            {
                Id = item.Jav.Id,
                Code = item.Jav.Code,
                Actress = item.Actress != null ? new ActressDto
                {
                    Id = item.Actress.Id,
                    Name = item.Actress.Name,
                    Image = item.Actress.Image,
                    Links = item.ActressLinks.Select(l => new LinkDto
                    {
                        Id = l.Id,
                        Url = l.Url
                    }).ToList()
                } : null,
                Image = item.Jav.Image,
                Status = item.Jav.Status,
                Links = item.JavLinks.Select(l => new LinkDto
                {
                    Id = l.Id,
                    Url = l.Url
                }).ToList(),
                CreatedAt = item.Jav.CreatedAt
            };
        }
    }
}
