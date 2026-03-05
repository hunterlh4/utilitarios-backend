using MediatR;
using UtilitariosCore.Application.Features.Proyects.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record GetProyectByIdQuery(int Id) : IRequest<Result<ProyectDetailDto>>;

internal sealed class GetProyectByIdQueryHandler(
    IProyectRepository proyectRepository,
    IMediaRepository mediaRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetProyectByIdQuery, Result<ProyectDetailDto>>
{
    public async Task<Result<ProyectDetailDto>> Handle(GetProyectByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await proyectRepository.GetById(request.Id);
        if (item is null) return Errors.NotFound("Proyecto no encontrado.");

        var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.Project);
        var links = await linkRepository.GetLinksByRefId(item.Id, LinkType.Project);
        var tags = await tagRepository.GetTagsByRefId(item.Id, TagType.Project);

        return new ProyectDetailDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Url = item.Url,
            Media = media.OrderBy(m => m.OrderIndex).Select(m => new ProyectMediaDto
            {
                Id = m.Id,
                Url = m.Url,
                Thumbnail = m.Thumbnail,
                OrderIndex = m.OrderIndex
            }).ToList(),
            Links = links.Select(l => new ProyectLinkDto
            {
                Id = l.Id,
                Name = l.Name,
                Url = l.Url,
                OrderIndex = l.OrderIndex
            }).ToList(),
            Tags = tags.Select(t => t.Name).ToList(),
            CreatedAt = item.CreatedAt
        };
    }
}
