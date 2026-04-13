using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record GetAllGirlGaleriesQuery : IRequest<Result<IEnumerable<GirlGaleryDto>>>;

internal sealed class GetAllGirlGaleriesQueryHandler(IGaleryRepository repository)
    : IRequestHandler<GetAllGirlGaleriesQuery, Result<IEnumerable<GirlGaleryDto>>>
{
    public async Task<Result<IEnumerable<GirlGaleryDto>>> Handle(GetAllGirlGaleriesQuery request, CancellationToken cancellationToken)
    {
        var result = (await repository.GetAllGirlGaleries())
            .Select(item => new GirlGaleryDto
            {
                Id = item.Id,
                Name = item.Name,
                Image = item.Image,
                CreatedAt = item.CreatedAt
            });
        return result.ToList();
    }
}
