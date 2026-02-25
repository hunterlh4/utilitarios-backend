using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record GetTagsByTypeQuery(TagType Type) : IRequest<Result<IEnumerable<Tag>>>
{
    internal sealed class Handler(ITagRepository tagRepository)
        : IRequestHandler<GetTagsByTypeQuery, Result<IEnumerable<Tag>>>
    {
        public async Task<Result<IEnumerable<Tag>>> Handle(GetTagsByTypeQuery request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAllTagsByType(request.Type);
            return tags.ToList();
        }
    }
}
