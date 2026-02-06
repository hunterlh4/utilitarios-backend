using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record GetGirlGaleryByIdQuery(int Id) : IRequest<Result<GirlGaleryDetailDto>>;

internal sealed class GetGirlGaleryByIdQueryHandler(
    IGirlGaleryRepository repository,
    IMediaRepository mediaRepository) 
    : IRequestHandler<GetGirlGaleryByIdQuery, Result<GirlGaleryDetailDto>>
{
    public async Task<Result<GirlGaleryDetailDto>> Handle(GetGirlGaleryByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetGirlGaleryById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        var media = await mediaRepository.GetMediaByRefId(item.Id, MediaType.GirlGalery);
        var mediaList = media
            .OrderBy(m => m.OrderIndex)
            .Skip(1) // Excluir la primera imagen
            .Select(m => new MediaDto
            {
                Id = m.Id,
                Url = m.Url,
                Thumbnail = m.Thumbnail,
                OrderIndex = m.OrderIndex
            })
            .ToList();

        var result = new GirlGaleryDetailDto
        {
            Id = item.Id,
            Name = item.Name,
            Media = mediaList,
            CreatedAt = item.CreatedAt
        };

        return result;
    }
}
