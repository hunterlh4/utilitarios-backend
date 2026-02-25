using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record GetJavByIdQuery(int Id) : IRequest<Result<JavDto>>
{
    internal sealed class Handler(
        IJavRepository javRepository,
        ITagRepository tagRepository)
        : IRequestHandler<GetJavByIdQuery, Result<JavDto>>
    {
        public async Task<Result<JavDto>> Handle(GetJavByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavWithDetailsById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            var actresses = new List<ActressDto>();
            foreach (var actressWithLinks in item.Actresses)
            {
                var tags = await tagRepository.GetTagsByRefId(actressWithLinks.Actress.Id, TagType.ActressJav);
                actresses.Add(new ActressDto
                {
                    Id = actressWithLinks.Actress.Id,
                    Name = actressWithLinks.Actress.Name,
                    Image = actressWithLinks.Actress.Image,
                    Tags = tags.Select(t => t.Name).ToList(),
                    Links = actressWithLinks.Links.Select(l => new LinkDto
                    {
                        Id = l.Id,
                        Url = l.Url
                    }).ToList()
                });
            }

            return new JavDto
            {
                Id = item.Jav.Id,
                Code = item.Jav.Code,
                Actresses = actresses,
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
