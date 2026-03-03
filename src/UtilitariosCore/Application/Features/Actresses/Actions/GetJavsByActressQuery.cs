using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record GetJavsByActressQuery(int ActressId) : IRequest<Result<IEnumerable<JavSummaryDto>>>;

internal sealed class GetJavsByActressQueryHandler(
    IJavRepository javRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetJavsByActressQuery, Result<IEnumerable<JavSummaryDto>>>
{
    public async Task<Result<IEnumerable<JavSummaryDto>>> Handle(GetJavsByActressQuery request, CancellationToken cancellationToken)
    {
        var javsWithDetails = await javRepository.GetJavsWithDetailsByActressId(request.ActressId);

        var result = new List<JavSummaryDto>();
        foreach (var item in javsWithDetails)
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var javTags = await tagRepository.GetTagsByRefId(item.Jav.Id, TagType.Jav);
            foreach (var tag in javTags) allTags.Add(tag.Name);

            var actressSummaries = new List<JavActressSummaryDto>();
            foreach (var actressWithLinks in item.Actresses)
            {
                var aTags = await tagRepository.GetTagsByRefId(actressWithLinks.Actress.Id, TagType.ActressJav);
                foreach (var tag in aTags) allTags.Add(tag.Name);
                actressSummaries.Add(new JavActressSummaryDto
                {
                    Id = actressWithLinks.Actress.Id,
                    Name = actressWithLinks.Actress.Name
                });
            }

            result.Add(new JavSummaryDto
            {
                Id = item.Jav.Id,
                Code = item.Jav.Code,
                Image = item.Jav.Image,
                Status = item.Jav.Status,
                Tags = [.. allTags],
                Actresses = actressSummaries,
                Links = item.JavLinks.Select(l => l.Url).ToList()
            });
        }

        return result;
    }
}
