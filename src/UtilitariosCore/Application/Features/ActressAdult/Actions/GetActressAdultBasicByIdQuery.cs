using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetActressAdultBasicByIdQuery(int Id) : IRequest<Result<ActressAdultBasicDto>>;

internal sealed class GetActressAdultBasicByIdQueryHandler(
    IActressAdultRepository actressAdultRepository,
    IMediaRepository mediaRepository,
    ITagRepository tagRepository) 
    : IRequestHandler<GetActressAdultBasicByIdQuery, Result<ActressAdultBasicDto>>
{
    public async Task<Result<ActressAdultBasicDto>> Handle(GetActressAdultBasicByIdQuery request, CancellationToken cancellationToken)
    {
        var actress = await actressAdultRepository.GetActressAdultById(request.Id);
        
        if (actress == null)
        {
            return Errors.NotFound("Actress not found.");
        }

        // Obtener imágenes
        var media = await mediaRepository.GetMediaByRefId(request.Id, MediaType.ActressAdult);
        
        // Obtener tags
        var tags = await tagRepository.GetTagsByRefId(request.Id, TagType.ActressAdult);
        
        var result = new ActressAdultBasicDto
        {
            Id = actress.Id,
            Name = actress.Name,
            Images = media.OrderBy(m => m.OrderIndex).Select(m => new MediaDto
            {
                Id = m.Id,
                Url = m.Url,
                OrderIndex = m.OrderIndex
            }).ToList(),
            TagIds = tags.Select(t => t.Id).ToList()
        };

        return result;
    }
}
