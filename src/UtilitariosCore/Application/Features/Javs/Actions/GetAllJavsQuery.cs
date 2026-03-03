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
                var actresses = new List<ActressDto>();
                var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                var javTags = await tagRepository.GetTagsByRefId(item.Jav.Id, TagType.Jav);
                foreach (var tag in javTags) allTags.Add(tag.Name);

                foreach (var actressWithLinks in item.Actresses)
                {
                    var tags = await tagRepository.GetTagsByRefId(actressWithLinks.Actress.Id, TagType.ActressJav);
                    foreach (var tag in tags) allTags.Add(tag.Name);
                    actresses.Add(new ActressDto
                    {
                        Id = actressWithLinks.Actress.Id,
                        Name = actressWithLinks.Actress.Name
                    });
                }

                result.Add(new JavDto
                {
                    Id = item.Jav.Id,
                    Code = item.Jav.Code,
                    Actresses = actresses,
                    Tags = [.. allTags],
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
