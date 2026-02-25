using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class GetAllJavsQuery : IRequest<Result<IEnumerable<JavDto>>>
{
    internal sealed class Handler(
        IJavRepository javRepository,
        ITagRepository tagRepository)
        : IRequestHandler<GetAllJavsQuery, Result<IEnumerable<JavDto>>>
    {
        public async Task<Result<IEnumerable<JavDto>>> Handle(GetAllJavsQuery request, CancellationToken cancellationToken)
        {
            var items = await javRepository.GetAllJavsWithDetails();

            var result = new List<JavDto>();
            foreach (var item in items)
            {
                List<string> actressTags = [];
                if (item.Actress != null)
                {
                    var tags = await tagRepository.GetTagsByRefId(item.Actress.Id, TagType.ActressJav);
                    actressTags = tags.Select(t => t.Name).ToList();
                }

                result.Add(new JavDto
                {
                    Id = item.Jav.Id,
                    Code = item.Jav.Code,
                    Actress = item.Actress != null ? new ActressDto
                    {
                        Id = item.Actress.Id,
                        Name = item.Actress.Name,
                        Image = item.Actress.Image,
                        Tags = actressTags,
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
                });
            }

            return result;
        }
    }
}
