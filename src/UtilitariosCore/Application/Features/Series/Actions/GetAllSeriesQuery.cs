using MediatR;
using UtilitariosCore.Application.Features.Series.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record GetAllSeriesQuery : IRequest<Result<IEnumerable<SeriesDto>>>;

internal sealed class GetAllSeriesQueryHandler(ISeriesRepository seriesRepository) 
    : IRequestHandler<GetAllSeriesQuery, Result<IEnumerable<SeriesDto>>>
{
    public async Task<Result<IEnumerable<SeriesDto>>> Handle(GetAllSeriesQuery request, CancellationToken cancellationToken)
    {
        var items = await seriesRepository.GetAllSeries();

        var result = items.Select(x => new SeriesDto
        {
            Id = x.Id,
            ImdbId = x.ImdbId,
            Title = x.Title,
            Image = x.Image,
            Year = x.Year,
            Rating = x.Rating,
            Type = x.Type,
            Status = x.Status,
            CreatedAt = x.CreatedAt
        });

        return result.ToList();
    }
}
