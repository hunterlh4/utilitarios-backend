using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record ExportSeriesExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportSeriesExcelQueryHandler(ISeriesRepository repository)
    : IRequestHandler<ExportSeriesExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportSeriesExcelQuery request, CancellationToken cancellationToken)
    {
        var rows = (await repository.GetAllSeries())
            .Select(s => new SeriesExcelRow
            {
                Id = s.Id,
                ImdbId = s.ImdbId,
                Title = s.Title,
                Image = s.Image,
                Year = s.Year,
                Rating = s.Rating,
                Type = s.Type,
                Status = (int)s.Status,
            })
            .ToList();

        using var stream = ExcelHelper.CreateSeriesExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"series.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
