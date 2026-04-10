using MediatR;
using UtilitariosCore.Application.Features.DotaTreasures.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record GetAllTreasuresQuery : IRequest<Result<IEnumerable<DotaTreasureDto>>>;

internal sealed class GetAllDotaTreasuresQueryHandler(ISteamRepository repository)
    : IRequestHandler<GetAllTreasuresQuery, Result<IEnumerable<DotaTreasureDto>>>
{
    public async Task<Result<IEnumerable<DotaTreasureDto>>> Handle(GetAllTreasuresQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllTreasure();
        return items.Select(t => new DotaTreasureDto
        {
            Id = t.Id, 
            Name = t.Name, 
            Image = t.Image, 
            ImagePresentation = t.ImagePresentation,
            Year = t.Year, 
            Type = t.Type, 
            CreatedAt = t.CreatedAt
        }).ToList();
    }
}
