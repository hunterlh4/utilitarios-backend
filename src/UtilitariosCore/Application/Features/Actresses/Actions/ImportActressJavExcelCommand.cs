using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record ImportActressJavExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportActressJavExcelCommandHandler(IActressJavRepository repository)
    : IRequestHandler<ImportActressJavExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportActressJavExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadActressJavExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCase(row.Name);
            var existing = await repository.GetActressJavByName(normalizedName);

            if (existing is null)
            {
                await repository.CreateActressJav(new ActressJav
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                });
                created++;
                continue;
            }

            var nextImage = string.IsNullOrWhiteSpace(row.Image) ? existing.Image : row.Image;
            var hasChanges = existing.Name != normalizedName || existing.Image != nextImage;

            if (!hasChanges)
            {
                skipped++;
                continue;
            }

            existing.Name = normalizedName;
            existing.Image = nextImage;
            await repository.UpdateActressJav(existing);
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
