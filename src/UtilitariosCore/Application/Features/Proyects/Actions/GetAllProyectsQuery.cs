using MediatR;
using UtilitariosCore.Application.Features.Proyects.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Proyects.Actions;

public record GetAllProyectsQuery : IRequest<Result<IEnumerable<ProyectDto>>>;

internal sealed class GetAllProyectsQueryHandler(
    IProyectRepository proyectRepository,
    IMediaRepository mediaRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetAllProyectsQuery, Result<IEnumerable<ProyectDto>>>
{
    public async Task<Result<IEnumerable<ProyectDto>>> Handle(GetAllProyectsQuery request, CancellationToken cancellationToken)
    {
        var items = await proyectRepository.GetAll();
        var result = new List<ProyectDto>();

        foreach (var item in items)
        {
            var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.Project);
            var firstImage = media.OrderBy(m => m.OrderIndex).FirstOrDefault();

            var tags = await tagRepository.GetTagsByRefId(item.Id, TagType.Project);

            result.Add(new ProyectDto
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
