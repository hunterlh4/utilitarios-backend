using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public record ImportComicExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportComicExcelCommandHandler(IComicRepository repository)
    : IRequestHandler<ImportComicExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportComicExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacío.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadComicExcel(stream);

        int created = 0, updated = 0, skipped = 0, invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            if (row.Id > 0)
            {
                var existing = await repository.GetComicById(row.Id);
                if (existing is null) { invalid++; continue; }

                var hasChanges = existing.Name != row.Name.Trim()
                    || existing.Image != row.Image.Trim()
                    || existing.Url != row.Url.Trim()
                    || existing.Category != row.Category.Trim();

                if (!hasChanges) { skipped++; continue; }

                existing.Name = row.Name.Trim();
                existing.Image = row.Image.Trim();
                existing.Url = row.Url.Trim();
                existing.Category = row.Category.Trim();
                await repository.UpdateComic(existing);
                updated++;
            }
            else
            {
                await repository.CreateComic(new Comic
                {
                    Name = row.Name.Trim(),
                    Image = row.Image.Trim(),
                    Url = row.Url.Trim(),
                    Category = row.Category.Trim(),
                    CreatedAt = DateTime.UtcNow,
                });
                created++;
            }
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
