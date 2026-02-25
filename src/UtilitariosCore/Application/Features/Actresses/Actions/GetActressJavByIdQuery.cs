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
    ILinkRepository linkRepository)
    : IRequestHandler<GetActressJavByIdQuery, Result<ActressJavDetailDto>>
{
    public async Task<Result<ActressJavDetailDto>> Handle(GetActressJavByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressRepository.GetActressWithTagsById(request.Id);
        if (actress == null) return Errors.NotFound("Actriz no encontrada.");

        var links = await linkRepository.GetLinksByRefId(request.Id, LinkType.ActressJav);
        var javs = await javRepository.GetJavsByActressId(request.Id);

        return new ActressJavDetailDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Image = actress.Image,
            CreatedAt = actress.CreatedAt,
            Tags = actress.Tags,
            Links = links.Select(l => new ActressLinkDto
            {
                Id = l.Id,
                Url = l.Url,
                OrderIndex = l.OrderIndex
            }).ToList(),
            Javs = javs.Select(j => new JavSummaryDto
            {
                Id = j.Id,
                Code = j.Code,
                Image = j.Image,
                Status = j.Status
            }).ToList()
        };
    }
}
