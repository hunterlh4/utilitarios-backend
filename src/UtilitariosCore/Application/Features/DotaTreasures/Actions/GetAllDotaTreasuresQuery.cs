using MediatR;
using UtilitariosCore.Application.Features.DotaTreasures.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record GetAllDotaTreasuresQuery : IRequest<Result<IEnumerable<DotaTreasureDto>>>;

internal sealed class GetAllDotaTreasuresQueryHandler(IDotaTreasureRepository repository)
    : IRequestHandler<GetAllDotaTreasuresQuery, Result<IEnumerable<DotaTreasureDto>>>
{
    public async Task<Result<IEnumerable<DotaTreasureDto>>> Handle(GetAllDotaTreasuresQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAll();
        return items.Select(t => new DotaTreasureDto
        {
            Id = t.Id, Name = t.Name, Image = t.Image, ImagePresentation = t.ImagePresentation,
            Year = t.Year, Type = t.Type, CreatedAt = t.CreatedAt
        }).ToList();
    }
}
