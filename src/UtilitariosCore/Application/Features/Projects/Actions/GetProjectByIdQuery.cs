using MediatR;
using UtilitariosCore.Application.Features.Projects.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public record GetProjectByIdQuery(int Id) : IRequest<Result<ProjectDetailDto>>
{
    internal sealed class Handler(
        IProjectRepository projectRepository,
        IMediaRepository mediaRepository,
        ILinkRepository linkRepository,
        ITagRepository tagRepository)
        : IRequestHandler<GetProjectByIdQuery, Result<ProjectDetailDto>>
    {
        public async Task<Result<ProjectDetailDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await projectRepository.GetById(request.Id);
            if (item is null) return Errors.NotFound("Proyecto no encontrado.");

            var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.Project);
            var links = await linkRepository.GetLinksByRefId(item.Id, LinkType.Project);
            var tags = await tagRepository.GetTagsByRefId(item.Id, TagType.Project);

            return new ProjectDetailDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Url = item.Url,
                Media = media.OrderBy(m => m.OrderIndex).Select(m => new ProjectMediaDto
                {
                    Id = m.Id,
                    Url = m.Url,
                    Thumbnail = m.Thumbnail,
                    OrderIndex = m.OrderIndex
                }).ToList(),
                Links = links.Select(l => new ProjectLinkDto
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
}
