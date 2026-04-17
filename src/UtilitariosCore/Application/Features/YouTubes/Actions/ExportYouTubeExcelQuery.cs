using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.YouTubes.Actions;

public record ExportYouTubeExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportYouTubeExcelQueryHandler(IYouTubeRepository repository)
    : IRequestHandler<ExportYouTubeExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportYouTubeExcelQuery request, CancellationToken cancellationToken)
    {
        var rows = (await repository.GetAll())
            .Select(y => new YouTubeExcelRow
            {
                Id = y.Id,
                Url = y.Url,
                Title = y.Title,
                AuthorName = y.AuthorName,
                AuthorUrl = y.AuthorUrl,
                Type = y.Type,
                Height = y.Height,
                Width = y.Width,
                Version = y.Version,
                ProviderName = y.ProviderName,
                ProviderUrl = y.ProviderUrl,
                ThumbnailHeight = y.ThumbnailHeight,
                ThumbnailWidth = y.ThumbnailWidth,
                ThumbnailUrl = y.ThumbnailUrl,
                Html = y.Html,
                Category = (int)y.Category,
            })
            .ToList();

        using var stream = ExcelHelper.CreateYouTubeExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"youtube-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
