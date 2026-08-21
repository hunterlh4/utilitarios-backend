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

            // Mapear actrices (sin sus links)
            var actresses = item.Actresses.Select(actressWithLinks => new ActressDto
            {
                Id = actressWithLinks.Actress.Id,
                Name = actressWithLinks.Actress.Name,
                CreatedAt = actressWithLinks.Actress.CreatedAt
            }).ToList();

            // Mapear links del JAV
            var javLinks = item.JavLinks
                .OrderBy(l => l.OrderIndex ?? int.MaxValue)
                .Select(l => new LinkJavDto
                {
                    Id = l.Id,
                    JavId = l.JavId,
                    Url = l.Url,
                    OrderIndex = l.OrderIndex,
                    CreatedAt = l.CreatedAt
                }).ToList();

            var javTags = await tagRepository.GetTagsByRefId(item.Jav.Id, TagType.Jav);

            return new JavDto
            {
                Id = item.Jav.Id,
                Code = item.Jav.Code,
                Actresses = actresses,
                Tags = javTags.Select(t => t.Name).ToList(),
                Image = item.Jav.Image,
                Status = item.Jav.Status,
                Links = javLinks,
                CreatedAt = item.Jav.CreatedAt
            };
        }
    }
}
