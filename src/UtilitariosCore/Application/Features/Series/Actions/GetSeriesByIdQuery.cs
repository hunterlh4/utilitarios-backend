using MediatR;
using UtilitariosCore.Application.Features.Series.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record GetSeriesByIdQuery(int Id) : IRequest<Result<SeriesDto>>;

internal sealed class GetSeriesByIdQueryHandler(ISeriesRepository seriesRepository) 
    : IRequestHandler<GetSeriesByIdQuery, Result<SeriesDto>>
{
    public async Task<Result<SeriesDto>> Handle(GetSeriesByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await seriesRepository.GetSeriesById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        var result = new SeriesDto
        {
            Id = item.Id,
            ImdbId = item.ImdbId,
            Title = item.Title,
            Image = item.Image,
            Year = item.Year,
            Rating = item.Rating,
            Type = item.Type,
            Status = item.Status,
            CreatedAt = item.CreatedAt
        };

        return result;
    }
}
