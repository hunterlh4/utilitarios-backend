using MediatR;
using UtilitariosCore.Application.Features.Comics.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public record GetAllComicsQuery : IRequest<Result<IEnumerable<ComicDto>>>;

internal sealed class GetAllComicsQueryHandler(IComicRepository repository)
    : IRequestHandler<GetAllComicsQuery, Result<IEnumerable<ComicDto>>>
{
    public async Task<Result<IEnumerable<ComicDto>>> Handle(GetAllComicsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllComics();
        return items.Select(x => new ComicDto
        {
            Id = x.Id,
            Name = x.Name,
            Image = x.Image,
            Url = x.Url,
            Category = x.Category,
            CreatedAt = x.CreatedAt
        }).ToList();
    }
}
