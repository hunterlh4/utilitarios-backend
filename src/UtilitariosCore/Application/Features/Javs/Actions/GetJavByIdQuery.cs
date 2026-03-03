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

            var actresses = item.Actresses.Select(a => new ActressDto
            {
                Id = a.Actress.Id,
                Name = a.Actress.Name
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
