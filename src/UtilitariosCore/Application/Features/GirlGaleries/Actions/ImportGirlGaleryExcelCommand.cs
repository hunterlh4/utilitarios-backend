using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record ImportGirlGaleryExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportGirlGaleryExcelCommandHandler(
    IGaleryRepository repository,
    IMediaRepository mediaRepository)
    : IRequestHandler<ImportGirlGaleryExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportGirlGaleryExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var data = ExcelHelper.ReadGirlGaleryExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var idMap = new Dictionary<int, int>();

        foreach (var row in data.Galeries.OrderBy(r => r.Id <= 0 ? int.MaxValue : r.Id))
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCase(row.Name);
            var existing = row.Id > 0 ? await repository.GetGirlGaleryById(row.Id) : null;

            if (existing is null)
            {
                existing = await repository.GetGirlGaleryByName(normalizedName);
            }

            if (existing is null)
            {
                var newId = await repository.CreateGirlGalery(new GirlGalery
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow
                });

                created++;
                if (row.Id > 0)
                    idMap[row.Id] = newId;
                continue;
            }

            existing.Name = normalizedName;
            existing.Image = string.IsNullOrWhiteSpace(row.Image) ? existing.Image : row.Image;
            await repository.UpdateGirlGalery(existing);
            updated++;

            if (row.Id > 0)
                idMap[row.Id] = existing.Id;
        }

        foreach (var mediaRow in data.Media)
        {
            if (mediaRow.GaleryId <= 0 || string.IsNullOrWhiteSpace(mediaRow.Url))
            {
                invalid++;
                continue;
            }

            var galeryId = idMap.TryGetValue(mediaRow.GaleryId, out var mappedId) ? mappedId : mediaRow.GaleryId;
            var galery = await repository.GetGirlGaleryById(galeryId);
            if (galery is null)
            {
                skipped++;
                continue;
            }

            var existingMedia = await mediaRepository.GetMediaByRefId(galeryId, MediaType.GirlGalery);
            var existingItem = existingMedia.FirstOrDefault(m => string.Equals(m.Url, mediaRow.Url, StringComparison.OrdinalIgnoreCase));
            if (existingItem is not null)
            {
                if (mediaRow.OrderIndex > 0 && existingItem.OrderIndex != mediaRow.OrderIndex)
                {
                    await mediaRepository.UpdateMediaOrder(existingItem.Id, mediaRow.OrderIndex);
                }

                updated++;
                continue;
            }

            var nextOrder = mediaRow.OrderIndex > 0
                ? mediaRow.OrderIndex
                : (existingMedia.Any() ? existingMedia.Max(m => m.OrderIndex) + 1 : 1);

            await mediaRepository.CreateMedia(new UtilitariosCore.Domain.Models.Media
            {
                RefId = galeryId,
                Type = MediaType.GirlGalery,
                Url = mediaRow.Url,
                Thumbnail = null,
                OrderIndex = nextOrder,
                CreatedAt = DateTime.UtcNow,
            });
            created++;
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
