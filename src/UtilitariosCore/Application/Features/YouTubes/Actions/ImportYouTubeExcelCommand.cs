using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.YouTubes.Actions;

public record ImportYouTubeExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportYouTubeExcelCommandHandler(IYouTubeRepository repository)
    : IRequestHandler<ImportYouTubeExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportYouTubeExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadYouTubeExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var existingByUrl = (await repository.GetAll())
            .Where(y => !string.IsNullOrWhiteSpace(y.Url))
            .ToDictionary(y => y.Url.Trim(), StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Url) || string.IsNullOrWhiteSpace(row.Title))
            {
                invalid++;
                continue;
            }

            var url = row.Url.Trim();
            if (existingByUrl.ContainsKey(url))
            {
                skipped++;
                continue;
            }

            var category = Enum.IsDefined(typeof(YouTubeCategory), row.Category)
                ? (YouTubeCategory)row.Category
                    : YouTubeCategory.Anime;

            await repository.Create(new YouTube
            {
                Url = url,
                Title = row.Title.Trim(),
                AuthorName = row.AuthorName,
                AuthorUrl = row.AuthorUrl,
                Type = row.Type,
                Height = row.Height,
                Width = row.Width,
                Version = row.Version,
                ProviderName = row.ProviderName,
                ProviderUrl = row.ProviderUrl,
                ThumbnailHeight = row.ThumbnailHeight,
                ThumbnailWidth = row.ThumbnailWidth,
                ThumbnailUrl = row.ThumbnailUrl,
                Html = row.Html,
                Category = category,
                CreatedAt = DateTime.UtcNow,
            });

            created++;
            existingByUrl[url] = new Application.Features.YouTubes.Dtos.YouTubeDto { Url = url };
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }
}
