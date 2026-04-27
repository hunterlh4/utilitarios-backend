using MediatR;
using UtilitariosCore.Application.Features.Projects.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public record GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
{
    internal sealed class Handler(
        IProjectRepository projectRepository,
        IMediaRepository mediaRepository,
        ITagRepository tagRepository)
        : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var items = await projectRepository.GetAll();
            var result = new List<ProjectDto>();

            foreach (var item in items)
            {
                var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.Project);
                var firstImage = media.OrderBy(m => m.OrderIndex).FirstOrDefault();
                var tags = await tagRepository.GetTagsByRefId(item.Id, TagType.Project);

                result.Add(new ProjectDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Url = item.Url,
                    FirstImageUrl = firstImage?.Url,
                    Tags = tags.Select(t => t.Name).ToList(),
                    CreatedAt = item.CreatedAt
                });
            }

            return result;
        }
    }
}
