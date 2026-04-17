using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record ImportAnimeExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportAnimeExcelCommandHandler(IAnimeRepository repository)
    : IRequestHandler<ImportAnimeExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportAnimeExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadAnimeExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.ApiId) || string.IsNullOrWhiteSpace(row.Title))
            {
                invalid++;
                continue;
            }

            var existing = await repository.GetAnimeByApiId(row.ApiId.Trim());
            var status = Enum.IsDefined(typeof(ContentStatus), row.Status)
                ? (ContentStatus)row.Status
                : ContentStatus.Pending;

            if (existing is null)
            {
                await repository.CreateAnime(new Anime
                {
                    ApiId = row.ApiId.Trim(),
                    Title = row.Title.Trim(),
                    Image = row.Image?.Trim() ?? string.Empty,
                    Episodes = row.Episodes,
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
                existing.Episodes != row.Episodes ||
                existing.Status != status;

            if (!hasChanges)
            {
                skipped++;
                continue;
            }

            existing.Title = nextTitle;
            existing.Image = nextImage;
            existing.Episodes = row.Episodes;
            existing.Status = status;
            await repository.UpdateAnime(existing);
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
