using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record GetActressJavByIdQuery(int Id) : IRequest<Result<ActressJavDetailDto>>;

internal sealed class GetActressJavByIdQueryHandler(
    IActressJavRepository actressRepository,
    IJavRepository javRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetActressJavByIdQuery, Result<ActressJavDetailDto>>
{
    public async Task<Result<ActressJavDetailDto>> Handle(GetActressJavByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressRepository.GetActressJavWithTagsById(request.Id);
        if (actress == null) return Errors.NotFound("Actriz no encontrada.");

        var links = await linkRepository.GetLinksByRefId(request.Id, LinkType.ActressJav);
        var javsWithDetails = await javRepository.GetJavsWithDetailsByActressId(request.Id);

        var javSummaries = new List<JavSummaryDto>();
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

            javSummaries.Add(new JavSummaryDto
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

        return new ActressJavDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Image = actress.Image ?? "",
            CreatedAt = actress.CreatedAt,
            Tags = actress.Tags,
            Links = links.OrderBy(l => l.OrderIndex ?? int.MaxValue).ToList(),
            Javs = javSummaries
        };
    }
}
