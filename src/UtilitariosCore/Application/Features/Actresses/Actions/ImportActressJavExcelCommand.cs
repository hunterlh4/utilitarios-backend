using MediatR;
using UtilitariosCore.Domain.Enums;
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

internal sealed class ImportActressJavExcelCommandHandler(IActressJavRepository repository, ILinkRepository linkRepository)
    : IRequestHandler<ImportActressJavExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportActressJavExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var excelData = ExcelHelper.ReadActressJavExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var actressesById = new Dictionary<int, ActressJav>();
        var actressesByName = new Dictionary<string, ActressJav>(StringComparer.OrdinalIgnoreCase);

        var existingActresses = await repository.GetAllActressJav();
        foreach (var actress in existingActresses)
        {
            actressesById[actress.Id] = actress;
            actressesByName[StringNormalizer.ToTitleCase(actress.Name)] = actress;
        }

        foreach (var row in excelData.Actresses)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCase(row.Name);
            ActressJav? existing = null;

            if (row.Id > 0 && actressesById.TryGetValue(row.Id, out var existingById))
            {
                existing = existingById;
            }
            else if (actressesByName.TryGetValue(normalizedName, out var existingByName))
            {
                existing = existingByName;
            }

            if (existing is null)
            {
                var actressId = await repository.CreateActressJav(new ActressJav
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                });

                var createdActress = new ActressJav
                {
                    Id = actressId,
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                };

                actressesById[actressId] = createdActress;
                actressesByName[normalizedName] = createdActress;

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
            actressesById[existing.Id] = existing;
            actressesByName[normalizedName] = existing;

            if (hasChanges)
                updated++;
        }

        var linksByActressId = new Dictionary<int, List<ActressJavLinkExcelRow>>();
        foreach (var linkRow in excelData.Links)
        {
            var resolvedActress = ResolveActress(linkRow, actressesById, actressesByName);
            if (resolvedActress is null)
            {
                invalid++;
                continue;
            }

            if (!linksByActressId.TryGetValue(resolvedActress.Id, out var linkRows))
            {
                linkRows = new List<ActressJavLinkExcelRow>();
                linksByActressId[resolvedActress.Id] = linkRows;
            }

            linkRows.Add(linkRow);
        }

        foreach (var pair in linksByActressId)
        {
            await SyncLinksAsync(pair.Key, pair.Value, cancellationToken);
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }

    private ActressJav? ResolveActress(ActressJavLinkExcelRow linkRow, IDictionary<int, ActressJav> actressesById, IDictionary<string, ActressJav> actressesByName)
    {
        if (linkRow.ActressJavId > 0 && actressesById.TryGetValue(linkRow.ActressJavId, out var existingById))
            return existingById;

        var actressName = StringNormalizer.ToTitleCase(linkRow.ActressJavName ?? string.Empty);
        if (!string.IsNullOrWhiteSpace(actressName) && actressesByName.TryGetValue(actressName, out var existingByName))
            return existingByName;

        return null;
    }

    private async Task<bool> SyncLinksAsync(int actressId, List<ActressJavLinkExcelRow> links, CancellationToken cancellationToken)
    {
        if (links is null)
            return false;

        await linkRepository.DeleteLinksByRefId(actressId, LinkType.ActressJav);

        var orderedLinks = links
            .Where(link => !string.IsNullOrWhiteSpace(link.Url))
            .OrderBy(link => link.OrderIndex > 0 ? link.OrderIndex : int.MaxValue)
            .ToList();

        for (int i = 0; i < orderedLinks.Count; i++)
        {
            var url = orderedLinks[i].Url.Trim();
            if (string.IsNullOrWhiteSpace(url))
                continue;

            await linkRepository.CreateLink(new Link
            {
                Type = LinkType.ActressJav,
                RefId = actressId,
                Url = url,
                OrderIndex = i + 1,
                CreatedAt = DateTime.UtcNow
            });
        }

        return true;
    }
}
