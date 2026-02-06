using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record GetAllGirlGaleriesQuery : IRequest<Result<IEnumerable<GirlGaleryDto>>>;

internal sealed class GetAllGirlGaleriesQueryHandler(
    IGirlGaleryRepository repository,
    IMediaRepository mediaRepository) 
    : IRequestHandler<GetAllGirlGaleriesQuery, Result<IEnumerable<GirlGaleryDto>>>
{
    public async Task<Result<IEnumerable<GirlGaleryDto>>> Handle(GetAllGirlGaleriesQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllGirlGaleries();
        var result = new List<GirlGaleryDto>();

        foreach (var item in items)
        {
            var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.GirlGalery);
            var firstImage = media.OrderBy(m => m.OrderIndex).FirstOrDefault();

            result.Add(new GirlGaleryDto
            {
                Id = item.Id,
                Name = item.Name,
                FirstImageUrl = firstImage?.Url,
                CreatedAt = item.CreatedAt
            });
        }

        return result.ToList();
    }
}
