using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record ImportSeriesExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportSeriesExcelCommandHandler(ISeriesRepository repository)
    : IRequestHandler<ImportSeriesExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportSeriesExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadSeriesExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.ImdbId) || string.IsNullOrWhiteSpace(row.Title))
            {
                invalid++;
                continue;
            }

            var existing = await repository.GetSeriesByImdbId(row.ImdbId.Trim());
            var status = Enum.IsDefined(typeof(ContentStatus), row.Status)
                ? (ContentStatus)row.Status
                : ContentStatus.Pending;

            if (existing is null)
            {
                await repository.CreateSeries(new Domain.Models.Series
                {
                    ImdbId = row.ImdbId.Trim(),
                    Title = row.Title.Trim(),
                    Image = row.Image?.Trim() ?? string.Empty,
                    Year = row.Year,
                    Rating = row.Rating,
                    Type = row.Type,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                });
                created++;
                continue;
            }

            var nextTitle = row.Title.Trim();
            var nextImage = row.Image?.Trim() ?? existing.Image;
            var hasChanges =
                existing.Title != nextTitle ||
                existing.Image != nextImage ||
                existing.Year != row.Year ||
                existing.Rating != row.Rating ||
                existing.Type != row.Type ||
                existing.Status != status;

            if (!hasChanges)
            {
                skipped++;
                continue;
            }

            existing.Title = nextTitle;
            existing.Image = nextImage;
            existing.Year = row.Year;
            existing.Rating = row.Rating;
            existing.Type = row.Type;
            existing.Status = status;
            await repository.UpdateSeries(existing);
            updated++;
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
